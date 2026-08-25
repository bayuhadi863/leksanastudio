using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Constants;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Helpers;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Common.Services
{
    /// <summary>
    /// CRUD for content that exists in more than one language.
    ///
    /// Everything a translated module needs and would otherwise reimplement — and
    /// get subtly wrong once — lives here: writing translations as a set, keeping
    /// slugs unique within a language, recording every slug a page has ever had so
    /// the old one can still answer, stamping a publication date exactly once, and
    /// running long-form bodies through the block gate.
    ///
    /// The split it enforces: the entry row holds what reads the same in every
    /// language; the translation rows hold everything else.
    /// </summary>
    public abstract class BaseTranslatableCrudService<
        TEntity,
        TTranslation,
        TDTO,
        TParam,
        TTranslationParam,
        TPaginationDTO,
        TFilterParam
    > : BaseCrudService<TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        where TEntity : class, ITranslatableEntity<TTranslation>
        where TTranslation : class, ITranslationEntity, new()
        where TDTO : class
        where TParam : class, ITranslatableParam<TTranslationParam>
        where TTranslationParam : class, ITranslationParam
        where TPaginationDTO : ContentPaginationDtoBase
        where TFilterParam : BasePaginationParam
    {
        protected readonly IBaseRepository<TTranslation> _translations;
        protected readonly ILocaleService _locales;
        private readonly ISlugHistoryRepository _slugHistory;
        private readonly IBlockDocumentProcessor _blocks;

        /// <summary>
        /// Maps the request onto the entry row and stops there.
        ///
        /// Without the exclusion, the mapper walks into <c>Translations</c> and
        /// builds a fresh set of translation objects with freshly generated ids —
        /// which the change tracker then tries to UPDATE, matching no rows. The
        /// translations are written deliberately below, one language at a time,
        /// because each needs its slug checked, its history recorded, and its
        /// publication date settled.
        /// </summary>
        private static readonly TypeAdapterConfig _entityOnly = BuildEntityOnlyConfig();

        private static TypeAdapterConfig BuildEntityOnlyConfig()
        {
            var config = new TypeAdapterConfig();
            config
                .ForType(typeof(TParam), typeof(TEntity))
                .Ignore(nameof(ITranslatableEntity<TTranslation>.Translations));
            return config;
        }

        protected BaseTranslatableCrudService(
            IBaseRepository<TEntity> repository,
            IBaseRepository<TTranslation> translations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, currentUserService)
        {
            _translations = translations;
            _slugHistory = slugHistory;
            _blocks = blocks;
            _locales = locales;
        }

        /// <summary>
        /// Module name, used to scope slug history and public lookups —
        /// e.g. <c>case-study</c>. Matches the menu code.
        /// </summary>
        protected abstract string ContentType { get; }

        /// <summary>
        /// Whether this module's addresses are unique across the whole site or only
        /// within the module. Case studies live under <c>/portofolio/…</c> so they
        /// only need to be unique among themselves; vertical pages sit at the root
        /// and would otherwise be able to shadow a real route.
        /// </summary>
        protected virtual bool SlugRequired => true;

        /* -------------------------------------------------------------- create */

        public override async Task<Guid> CreateAsync(TParam param)
        {
            return await _repository.ExecuteInTransactionAsync(async () =>
            {
                var entity = param.Adapt<TEntity>(_entityOnly);
                var actor = _currentUserService.UserName ?? AuditConstants.SystemUser;
                var now = DateTimeOffset.UtcNow;

                entity.CreatedDate = now;
                entity.CreatedBy = actor;
                entity.Translations = [];

                OnCreating(entity, param);

                var created = await _repository.CreateAsync(entity);

                foreach (var translationParam in param.Translations)
                {
                    var translation = new TTranslation();
                    translationParam.Adapt(translation);

                    translation.ParentId = created.Id;
                    translation.CreatedDate = now;
                    translation.CreatedBy = actor;

                    await NormaliseAsync(translation, translationParam, existing: null, actor, now);
                    await _translations.CreateAsync(translation);
                }

                return created.Id;
            });
        }

        /* -------------------------------------------------------------- update */

        public override async Task<Guid> UpdateAsync(Guid id, TParam param)
        {
            return await _repository.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _repository.GetAsync(q => q.Where(e => e.Id == id));
                if (entity == null)
                    throw new NotFoundException($"{typeof(TEntity).Name} dengan ID {id} tidak ditemukan");

                var actor = _currentUserService.UserName ?? AuditConstants.SystemUser;
                var now = DateTimeOffset.UtcNow;

                param.Adapt(entity, _entityOnly);
                entity.UpdatedDate = now;
                entity.UpdatedBy = actor;

                OnUpdating(entity, param);
                await _repository.UpdateAsync(entity);

                var stored = (await _translations.GetListAsync(q => q.Where(t => t.ParentId == id)))
                    .ToList();

                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var translationParam in param.Translations)
                {
                    seen.Add(translationParam.LocaleCode);

                    var existing = stored.FirstOrDefault(t =>
                        string.Equals(t.LocaleCode, translationParam.LocaleCode, StringComparison.OrdinalIgnoreCase)
                    );

                    if (existing is null)
                    {
                        var translation = new TTranslation();
                        translationParam.Adapt(translation);

                        translation.ParentId = id;
                        translation.CreatedDate = now;
                        translation.CreatedBy = actor;

                        await NormaliseAsync(translation, translationParam, existing: null, actor, now);
                        await _translations.CreateAsync(translation);
                        continue;
                    }

                    var previousSlug = existing.Slug;
                    var previousStatus = existing.Status;
                    var previousPublishedAt = existing.PublishedAt;

                    translationParam.Adapt(existing);

                    existing.Status = translationParam.Status;
                    existing.PublishedAt = previousPublishedAt;
                    existing.UpdatedDate = now;
                    existing.UpdatedBy = actor;

                    await NormaliseAsync(
                        existing,
                        translationParam,
                        new PreviousState(previousSlug, previousStatus),
                        actor,
                        now
                    );

                    await _translations.UpdateAsync(existing);
                }

                // A language dropped from the request is unpublished, not erased:
                // the writing stays recoverable, the page stops being served.
                var removed = stored.Where(t => !seen.Contains(t.LocaleCode)).ToList();
                foreach (var translation in removed)
                {
                    translation.IsDeleted = true;
                    translation.DeletedDate = now;
                    translation.DeletedBy = actor;
                }
                if (removed.Count > 0)
                    await _translations.UpdateRangeAsync(removed);

                return entity.Id;
            });
        }

        /* ----------------------------------------------------------- normalise */

        private sealed record PreviousState(string? Slug, ContentStatus Status);

        private async Task NormaliseAsync(
            TTranslation translation,
            TTranslationParam param,
            PreviousState? existing,
            string actor,
            DateTimeOffset now
        )
        {
            translation.LocaleCode = param.LocaleCode.Trim().ToLowerInvariant();

            await NormaliseSlugAsync(translation, existing, actor, now);
            NormaliseBody(translation);
            NormalisePublication(translation, existing, now);

            OnTranslationSaving(translation, param);
        }

        private async Task NormaliseSlugAsync(
            TTranslation translation,
            PreviousState? existing,
            string actor,
            DateTimeOffset now
        )
        {
            if (!SlugRequired)
                return;

            var slug = SlugHelper.Slugify(translation.Slug ?? FallbackSlugSource(translation));

            if (string.IsNullOrEmpty(slug))
                throw new BadRequestException("Alamat halaman wajib diisi.");

            var taken = await _translations.CountAsync(q =>
                q.Where(t =>
                    t.LocaleCode == translation.LocaleCode
                    && t.Slug == slug
                    && t.Id != translation.Id
                )
            );

            if (taken > 0)
            {
                throw new ConflictException(
                    $"Alamat \"{slug}\" sudah dipakai halaman lain dalam bahasa yang sama. "
                        + "Ubah salah satunya."
                );
            }

            // Every address a page has ever had is kept, so the old one can answer
            // 301 instead of 404. Without this, tidying a headline quietly kills an
            // indexed URL and nobody finds out for months.
            if (existing?.Slug is { Length: > 0 } old && old != slug)
            {
                await _slugHistory.CreateAsync(
                    new SlugHistory
                    {
                        EntityType = ContentType,
                        EntityId = translation.ParentId,
                        LocaleCode = translation.LocaleCode,
                        OldSlug = old,
                        NewSlug = slug,
                        CreatedDate = now,
                        CreatedBy = actor,
                    }
                );
            }

            translation.Slug = slug;
        }

        private void NormaliseBody(TTranslation translation)
        {
            if (translation is not IHasBlockBody withBody)
                return;

            var result = _blocks.Process(withBody.Body);
            if (!result.IsValid)
                throw new BadRequestException(result.Error!);

            withBody.Body = result.Json;
        }

        private static void NormalisePublication(
            TTranslation translation,
            PreviousState? existing,
            DateTimeOffset now
        )
        {
            var wasPublished = existing?.Status == ContentStatus.Published;

            // Stamped once, on the first publication. Re-publishing after an edit
            // must not make a two-year-old case study look like today's news.
            if (translation.Status == ContentStatus.Published && !wasPublished)
                translation.PublishedAt ??= now;

            if (translation.Status == ContentStatus.Draft)
                translation.PublishedAt = null;
        }

        /// <summary>
        /// Where to derive a slug from when the request left it blank — usually the
        /// title. Returning null makes the slug genuinely required.
        /// </summary>
        protected virtual string? FallbackSlugSource(TTranslation translation) => null;

        /// <summary>
        /// Last chance to adjust one translation before it is written. Runs inside
        /// the transaction, after slug, body, and publication have been settled.
        /// </summary>
        protected virtual void OnTranslationSaving(TTranslation translation, TTranslationParam param) { }

        /* ------------------------------------------------------------- reading */

        /// <summary>Loads an entry with every translation attached.</summary>
        protected Task<TEntity?> GetWithTranslationsAsync(Guid id) =>
            _repository.GetAsync(q => q.Include(e => e.Translations).Where(e => e.Id == id));

        /// <summary>
        /// Loads a published entry by its address in one language. The only lookup
        /// the public site ever performs.
        /// </summary>
        protected Task<TEntity?> GetPublishedBySlugAsync(string localeCode, string slug) =>
            _repository.GetAsync(q =>
                q.Include(e => e.Translations)
                    .Where(e =>
                        e.Translations.Any(t =>
                            t.LocaleCode == localeCode
                            && t.Slug == slug
                            && t.Status == ContentStatus.Published
                            && !t.IsDeleted
                        )
                    )
            );

        /// <summary>The address a retired slug now points to, or null if it never existed.</summary>
        public async Task<string?> ResolveMovedSlugAsync(string localeCode, string oldSlug)
        {
            var history = await _slugHistory.FindAsync(ContentType, localeCode, oldSlug);
            return history?.NewSlug;
        }

        /* ------------------------------------------------------- list plumbing */

        /// <summary>
        /// Content lists read in display order unless asked otherwise.
        ///
        /// Without this the repository falls back to newest-first, which quietly
        /// contradicts the one question these screens exist to answer — what does a
        /// visitor see first? — and would make a reorder control lie.
        /// </summary>
        protected override (
            Expression<Func<TEntity, object?>>? orderBy,
            bool descending
        ) GetSortExpression(TFilterParam param)
        {
            if (!string.IsNullOrWhiteSpace(param.SortBy))
                return base.GetSortExpression(param);

            var property = typeof(TEntity).GetProperty(
                nameof(IHasOrder.Order),
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
            );

            if (property is null)
                return base.GetSortExpression(param);

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var access = Expression.Property(parameter, property);
            var converted = Expression.Convert(access, typeof(object));

            return (Expression.Lambda<Func<TEntity, object?>>(converted, parameter), false);
        }


        /// <summary>
        /// What to call one translation in a list, and what to derive a slug from.
        /// Usually the title; a module whose entries have no title says so here.
        /// </summary>
        protected virtual string? DisplayName(TTranslation translation) =>
            FallbackSlugSource(translation);

        /// <summary>
        /// Fills in the language row of every list item: which languages exist, how
        /// far each has got, and the title to show in the reader's language.
        ///
        /// Identical for every module, so it lives here rather than being written
        /// twelve times and drifting in three of them. A module that needs more —
        /// a cover path, a parent's name — overrides and calls this first.
        /// </summary>
        protected override async Task EnrichPaginationAsync(List<TPaginationDTO> items)
        {
            if (items.Count == 0)
                return;

            var ids = items.Select(i => i.Id).ToList();
            var translations = (
                await _translations.GetListAsync(q => q.Where(t => ids.Contains(t.ParentId)))
            )
                .GroupBy(t => t.ParentId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var defaultLocale = await _locales.GetDefaultCodeAsync();

            foreach (var item in items)
            {
                var forEntry = translations.GetValueOrDefault(item.Id, []);

                item.Translations = forEntry
                    .OrderBy(t => t.LocaleCode)
                    .Select(t => new TranslationSummaryDTO
                    {
                        LocaleCode = t.LocaleCode,
                        Title = DisplayName(t),
                        Slug = t.Slug,
                        Status = t.Status,
                        PublishedAt = t.PublishedAt,
                    })
                    .ToList();

                // The list reads in the default language, falling back to whatever
                // exists rather than showing a blank row for an entry that simply
                // has not been translated yet.
                var primary =
                    forEntry.FirstOrDefault(t => t.LocaleCode == defaultLocale)
                    ?? forEntry.FirstOrDefault();

                item.Title = primary is null ? null : DisplayName(primary);
                item.Slug = primary?.Slug;
            }
        }

        /// <summary>Every translation of one entry, shaped for the panel form.</summary>
        protected async Task<List<TTranslationDto>> LoadTranslationDtosAsync<TTranslationDto>(
            Guid parentId
        )
        {
            var translations = await _translations.GetListAsync(q =>
                q.Where(t => t.ParentId == parentId)
            );

            return translations.OrderBy(t => t.LocaleCode).Adapt<List<TTranslationDto>>();
        }

        /// <summary>
        /// Narrows a list to entries that have at least one language in this state.
        /// "Draft" therefore means "has something unpublished", which is the
        /// question an editor is actually asking.
        /// </summary>
        protected static IQueryable<TEntity> FilterByStatus(
            IQueryable<TEntity> query,
            ContentStatus? status
        ) =>
            status is { } wanted
                ? query.Where(e => e.Translations.Any(t => !t.IsDeleted && t.Status == wanted))
                : query;

        /// <summary>
        /// Applies a new display order in one write, so the list never shows two
        /// entries sharing a position.
        /// </summary>
        public virtual async Task ReorderAsync(ReorderParam param)
        {
            if (param.Ids.Count == 0)
                return;

            var entities = (
                await _repository.GetListAsync(q => q.Where(e => param.Ids.Contains(e.Id)))
            ).ToList();

            var position = param
                .Ids.Select((id, index) => (id, index))
                .ToDictionary(x => x.id, x => x.index);
            var now = DateTimeOffset.UtcNow;
            var actor = _currentUserService.UserName ?? AuditConstants.SystemUser;

            foreach (var entity in entities)
            {
                if (entity is not IHasOrder ordered)
                    continue;

                ordered.Order = position[entity.Id];
                entity.UpdatedDate = now;
                entity.UpdatedBy = actor;
            }

            await _repository.UpdateRangeAsync(entities);
        }

        /* ----------------------------------------------------- public plumbing */

        /// <summary>Published entries in one language, in display order.</summary>
        protected async Task<List<TEntity>> GetPublishedEntitiesAsync(string localeCode)
        {
            var entities = await _repository.GetListAsync(q =>
                q.Include(e => e.Translations)
                    .Where(e =>
                        e.Translations.Any(t =>
                            t.LocaleCode == localeCode
                            && t.Status == ContentStatus.Published
                            && !t.IsDeleted
                        )
                    )
            );

            return entities.OrderBy(e => e is IHasOrder ordered ? ordered.Order : 0).ToList();
        }

        /// <summary>Live translations of one entry, in no particular order.</summary>
        protected static List<TTranslation> LiveTranslations(TEntity entity) =>
            entity
                .Translations.Where(t => !t.IsDeleted && t.Status == ContentStatus.Published)
                .ToList();

        /// <summary>
        /// This entry's address in every language that has published it.
        ///
        /// Only those languages: an hreflang pointing at a page that does not exist
        /// is worse than no hreflang at all.
        /// </summary>
        protected static Dictionary<string, string> AlternatesOf(
            IEnumerable<TTranslation> liveTranslations
        ) =>
            liveTranslations
                .Where(t => t.Slug is { Length: > 0 })
                .ToDictionary(t => t.LocaleCode, t => t.Slug!);
    }
}
