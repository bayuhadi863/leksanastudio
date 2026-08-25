using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.CaseStudy;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// Case studies — the most important content on the site.
    ///
    /// A prospect judges the studio by these before reading a single claim
    /// elsewhere, which is why the card leads with the problem that was solved
    /// rather than the client's name, and why honest labelling is a column and not
    /// a convention.
    /// </summary>
    public class CaseStudyService
        : BaseTranslatableCrudService<
            CaseStudy,
            CaseStudyTranslation,
            CaseStudyDTO,
            CaseStudyParam,
            CaseStudyTranslationParam,
            CaseStudyPaginationDTO,
            CaseStudyPaginationParam
        >,
            ICaseStudyService
    {
        private readonly IBaseRepository<Domain.Entities.Media> _media;

        public CaseStudyService(
            IBaseRepository<CaseStudy> repository,
            IBaseRepository<CaseStudyTranslation> translations,
            IBaseRepository<Domain.Entities.Media> media,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService)
        {
            _media = media;
        }

        protected override string ContentType => "case-study";

        protected override string? FallbackSlugSource(CaseStudyTranslation translation) =>
            translation.Title;

        /* --------------------------------------------------------------- panel */

        protected override IQueryable<CaseStudy> ApplyFilter(
            IQueryable<CaseStudy> query,
            CaseStudyPaginationParam param
        )
        {
            if (param.Label is { } label)
                query = query.Where(c => c.Label == label);

            if (param.Year is { } year)
                query = query.Where(c => c.Year == year);

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(c =>
                    c.Translations.Any(t =>
                        !t.IsDeleted
                        && (
                            (t.Title != null && EF.Functions.ILike(t.Title, term))
                            || (t.Slug != null && EF.Functions.ILike(t.Slug, term))
                            || (t.Client != null && EF.Functions.ILike(t.Client, term))
                        )
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichPaginationAsync(List<CaseStudyPaginationDTO> items)
        {
            await base.EnrichPaginationAsync(items);

            if (items.Count == 0)
                return;

            var covers = await LoadCoverPathsForAsync(items.Select(i => i.CoverMediaId));

            foreach (var item in items)
            {
                if (item.CoverMediaId is { } coverId)
                    item.CoverMediaPath = covers.GetValueOrDefault(coverId);
            }
        }

        protected override async Task EnrichSingleAsync(CaseStudy entity, CaseStudyDTO dto)
        {
            dto.Translations = await LoadTranslationDtosAsync<CaseStudyTranslationDTO>(entity.Id);
            dto.CoverMediaPath = await ResolveCoverPathAsync(entity.CoverMediaId);
        }

        /* --------------------------------------------------------------- public */

        public async Task<IEnumerable<CaseStudyPublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);
            var covers = await LoadCoverPathsForAsync(entities.Select(c => c.CoverMediaId));

            return entities
                .Select(entity => ToPublic(entity, localeCode, covers))
                .OfType<CaseStudyPublicDTO>();
        }

        public async Task<CaseStudyPublicDTO?> GetPublicBySlugAsync(string localeCode, string slug)
        {
            var entity = await GetPublishedBySlugAsync(localeCode, slug);
            if (entity is null)
                return null;

            var covers = await LoadCoverPathsForAsync([entity.CoverMediaId]);
            return ToPublic(entity, localeCode, covers);
        }

        private static CaseStudyPublicDTO? ToPublic(
            CaseStudy entity,
            string localeCode,
            IReadOnlyDictionary<Guid, string> covers
        )
        {
            var live = LiveTranslations(entity);
            var translation = live.FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            return new CaseStudyPublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Slug = translation.Slug,
                Label = entity.Label,
                Figure = entity.Figure,
                CoverPath = entity.CoverMediaId is { } id ? covers.GetValueOrDefault(id) : null,
                CoverAlt = translation.CoverAlt,
                Year = entity.Year,
                Stack = entity.Stack.Adapt<System.Text.Json.JsonElement?>(),
                Order = entity.Order,
                Title = translation.Title,
                Summary = translation.Summary,
                Problem = translation.Problem,
                Client = translation.Client,
                Kind = translation.Kind,
                Duration = translation.Duration,
                Role = translation.Role,
                Metrics = translation.Metrics.Adapt<System.Text.Json.JsonElement?>(),
                Body = translation.Body.Adapt<System.Text.Json.JsonElement?>(),
                PublishedAt = translation.PublishedAt,
                UpdatedDate = translation.UpdatedDate ?? translation.CreatedDate,

                Alternates = AlternatesOf(live),
            };
        }

        /* ----------------------------------------------------------------- media */

        private async Task<string?> ResolveCoverPathAsync(Guid? mediaId)
        {
            if (mediaId is null)
                return null;

            var media = await _media.GetAsync(q => q.Where(m => m.Id == mediaId));
            return media?.ObjectPath;
        }

        private async Task<IReadOnlyDictionary<Guid, string>> LoadCoverPathsForAsync(
            IEnumerable<Guid?> mediaIds
        )
        {
            var ids = mediaIds.OfType<Guid>().Distinct().ToList();
            if (ids.Count == 0)
                return new Dictionary<Guid, string>();

            var media = await _media.GetListAsync(q => q.Where(m => ids.Contains(m.Id)));
            return media.ToDictionary(m => m.Id, m => m.ObjectPath);
        }
    }
}
