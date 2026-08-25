using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.Note;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// Technical notes — the studio's content engine (blueprint 08).
    ///
    /// A note earns attention by being specific about one decision, so the pillar
    /// is a column rather than a tag: three kinds of writing, and nothing that
    /// belongs to none of them.
    /// </summary>
    public class NoteService
        : BaseTranslatableCrudService<
            Note,
            NoteTranslation,
            NoteDTO,
            NoteParam,
            NoteTranslationParam,
            NotePaginationDTO,
            NotePaginationParam
        >,
            INoteService
    {
        public NoteService(
            IBaseRepository<Note> repository,
            IBaseRepository<NoteTranslation> translations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService) { }

        protected override string ContentType => "note";

        protected override string? FallbackSlugSource(NoteTranslation translation) =>
            translation.Title;

        /* --------------------------------------------------------------- panel */

        protected override IQueryable<Note> ApplyFilter(
            IQueryable<Note> query,
            NotePaginationParam param
        )
        {
            if (param.Pillar is { } pillar)
                query = query.Where(n => n.Pillar == pillar);

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(n =>
                    n.Translations.Any(t =>
                        !t.IsDeleted
                        && (
                            (t.Title != null && EF.Functions.ILike(t.Title, term))
                            || (t.Slug != null && EF.Functions.ILike(t.Slug, term))
                        )
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(Note entity, NoteDTO dto)
        {
            dto.Translations = await LoadTranslationDtosAsync<NoteTranslationDTO>(entity.Id);
        }

        /* -------------------------------------------------------------- public */

        public async Task<IEnumerable<NotePublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);
            return entities.Select(entity => ToPublic(entity, localeCode)).OfType<NotePublicDTO>();
        }

        public async Task<NotePublicDTO?> GetPublicBySlugAsync(string localeCode, string slug)
        {
            var entity = await GetPublishedBySlugAsync(localeCode, slug);
            return entity is null ? null : ToPublic(entity, localeCode);
        }

        private static NotePublicDTO? ToPublic(Note entity, string localeCode)
        {
            var live = LiveTranslations(entity);
            var translation = live.FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            return new NotePublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Slug = translation.Slug,
                Pillar = entity.Pillar,
                Order = entity.Order,
                Title = translation.Title,
                Summary = translation.Summary,
                Body = translation.Body.Adapt<System.Text.Json.JsonElement?>(),
                PublishedAt = translation.PublishedAt,
                UpdatedDate = translation.UpdatedDate ?? translation.CreatedDate,
                Alternates = AlternatesOf(live),
            };
        }
    }
}
