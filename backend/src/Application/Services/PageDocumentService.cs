using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.PageDocument;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// Pages that are mostly prose — the privacy policy, the terms, and their kind.
    ///
    /// They carry a block body because they have no fixed section layout, and a
    /// page code because the footer links to them by name rather than by address:
    /// the address is a content decision, the link is not.
    /// </summary>
    public class PageDocumentService
        : BaseTranslatableCrudService<
            PageDocument,
            PageDocumentTranslation,
            PageDocumentDTO,
            PageDocumentParam,
            PageDocumentTranslationParam,
            PageDocumentPaginationDTO,
            PageDocumentPaginationParam
        >,
            IPageDocumentService
    {
        public PageDocumentService(
            IBaseRepository<PageDocument> repository,
            IBaseRepository<PageDocumentTranslation> translations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService) { }

        protected override string ContentType => "page-document";

        protected override string? FallbackSlugSource(PageDocumentTranslation translation) =>
            translation.Title;

        protected override string? DisplayName(PageDocumentTranslation translation) =>
            translation.Title;

        /* --------------------------------------------------------------- panel */

        /// <summary>
        /// The page code is the identity the code links to, so it is normalised and
        /// kept unique here rather than trusted to whoever typed it.
        /// </summary>
        protected override void OnCreating(PageDocument entity, PageDocumentParam param)
        {
            entity.PageCode = NormaliseCode(param.PageCode);
        }

        protected override void OnUpdating(PageDocument entity, PageDocumentParam param)
        {
            entity.PageCode = NormaliseCode(param.PageCode);
        }

        private static string NormaliseCode(string pageCode)
        {
            var code = Common.Helpers.SlugHelper.Slugify(pageCode);
            if (string.IsNullOrEmpty(code))
                throw new BadRequestException("Kode halaman wajib diisi.");
            return code;
        }

        protected override IQueryable<PageDocument> ApplyFilter(
            IQueryable<PageDocument> query,
            PageDocumentPaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(d =>
                    EF.Functions.ILike(d.PageCode, term)
                    || d.Translations.Any(t =>
                        !t.IsDeleted && t.Title != null && EF.Functions.ILike(t.Title, term)
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(PageDocument entity, PageDocumentDTO dto)
        {
            dto.Translations = await LoadTranslationDtosAsync<PageDocumentTranslationDTO>(entity.Id);
        }

        /* -------------------------------------------------------------- public */

        public async Task<IEnumerable<PageDocumentPublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);
            return entities
                .Select(entity => ToPublic(entity, localeCode))
                .OfType<PageDocumentPublicDTO>();
        }

        public async Task<PageDocumentPublicDTO?> GetPublicBySlugAsync(
            string localeCode,
            string slug
        )
        {
            var entity = await GetPublishedBySlugAsync(localeCode, slug);
            return entity is null ? null : ToPublic(entity, localeCode);
        }

        public async Task<PageDocumentPublicDTO?> GetPublicByCodeAsync(
            string localeCode,
            string pageCode
        )
        {
            var code = pageCode.Trim().ToLowerInvariant();

            var entity = await _repository.GetAsync(q =>
                q.Include(d => d.Translations)
                    .Where(d =>
                        d.PageCode == code
                        && d.Translations.Any(t =>
                            t.LocaleCode == localeCode
                            && t.Status == ContentStatus.Published
                            && !t.IsDeleted
                        )
                    )
            );

            return entity is null ? null : ToPublic(entity, localeCode);
        }

        private static PageDocumentPublicDTO? ToPublic(PageDocument entity, string localeCode)
        {
            var live = LiveTranslations(entity);
            var translation = live.FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            return new PageDocumentPublicDTO
            {
                Id = entity.Id,
                PageCode = entity.PageCode,
                LocaleCode = translation.LocaleCode,
                Slug = translation.Slug,
                Title = translation.Title,
                Lead = translation.Lead,
                Body = translation.Body.Adapt<System.Text.Json.JsonElement?>(),
                MetaTitle = translation.MetaTitle,
                MetaDescription = translation.MetaDescription,
                PublishedAt = translation.PublishedAt,
                UpdatedDate = translation.UpdatedDate ?? translation.CreatedDate,
                Alternates = AlternatesOf(live),
            };
        }
    }
}
