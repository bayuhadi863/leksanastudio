using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.Service;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// The lines of work the studio sells.
    ///
    /// Each one names its audience and its floor price, because the most expensive
    /// conversation in this business is the one that reaches the price at the end.
    /// The proof case study is a relation rather than a hand-written link, so it
    /// cannot dangle when that case study's address changes.
    /// </summary>
    public class ServiceService
        : BaseTranslatableCrudService<
            Domain.Entities.Content.Service,
            ServiceTranslation,
            ServiceDTO,
            ServiceParam,
            ServiceTranslationParam,
            ServicePaginationDTO,
            ServicePaginationParam
        >,
            IServiceService
    {
        private readonly IBaseRepository<CaseStudyTranslation> _caseStudyTranslations;

        public ServiceService(
            IBaseRepository<Domain.Entities.Content.Service> repository,
            IBaseRepository<ServiceTranslation> translations,
            IBaseRepository<CaseStudyTranslation> caseStudyTranslations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService)
        {
            _caseStudyTranslations = caseStudyTranslations;
        }

        protected override string ContentType => "service";

        protected override string? FallbackSlugSource(ServiceTranslation translation) =>
            translation.Name;

        protected override string? DisplayName(ServiceTranslation translation) => translation.Name;

        /* --------------------------------------------------------------- panel */

        protected override IQueryable<Domain.Entities.Content.Service> ApplyFilter(
            IQueryable<Domain.Entities.Content.Service> query,
            ServicePaginationParam param
        )
        {
            if (param.PricingShape is { } shape)
                query = query.Where(s => s.PricingShape == shape);

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(s =>
                    s.Translations.Any(t =>
                        !t.IsDeleted
                        && (
                            (t.Name != null && EF.Functions.ILike(t.Name, term))
                            || (t.Audience != null && EF.Functions.ILike(t.Audience, term))
                            || (t.Slug != null && EF.Functions.ILike(t.Slug, term))
                        )
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(
            Domain.Entities.Content.Service entity,
            ServiceDTO dto
        )
        {
            dto.Translations = await LoadTranslationDtosAsync<ServiceTranslationDTO>(entity.Id);

            if (entity.CaseStudyId is { } caseStudyId)
            {
                var defaultLocale = await _locales.GetDefaultCodeAsync();
                var proof = await _caseStudyTranslations.GetAsync(q =>
                    q.Where(t => t.ParentId == caseStudyId && t.LocaleCode == defaultLocale)
                );
                dto.CaseStudyTitle = proof?.Title;
            }
        }

        /* -------------------------------------------------------------- public */

        public async Task<IEnumerable<ServicePublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);
            var proofs = await LoadProofsAsync(entities.Select(s => s.CaseStudyId), localeCode);

            return entities
                .Select(entity => ToPublic(entity, localeCode, proofs))
                .OfType<ServicePublicDTO>();
        }

        public async Task<ServicePublicDTO?> GetPublicBySlugAsync(string localeCode, string slug)
        {
            var entity = await GetPublishedBySlugAsync(localeCode, slug);
            if (entity is null)
                return null;

            var proofs = await LoadProofsAsync([entity.CaseStudyId], localeCode);
            return ToPublic(entity, localeCode, proofs);
        }

        private static ServicePublicDTO? ToPublic(
            Domain.Entities.Content.Service entity,
            string localeCode,
            IReadOnlyDictionary<Guid, CaseStudyTranslation> proofs
        )
        {
            var live = LiveTranslations(entity);
            var translation = live.FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            var proof =
                entity.CaseStudyId is { } id ? proofs.GetValueOrDefault(id) : null;

            return new ServicePublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Slug = translation.Slug,
                StartingPrice = entity.StartingPrice,
                PricingShape = entity.PricingShape,
                Order = entity.Order,
                Name = translation.Name,
                ShortName = translation.ShortName,
                Audience = translation.Audience,
                Headline = translation.Headline,
                Summary = translation.Summary,
                StartingPriceLabel = translation.StartingPriceLabel,
                Problems = translation.Problems.Adapt<System.Text.Json.JsonElement?>(),
                Deliverables = translation.Deliverables.Adapt<System.Text.Json.JsonElement?>(),
                Exclusions = translation.Exclusions.Adapt<System.Text.Json.JsonElement?>(),
                Faq = translation.Faq.Adapt<System.Text.Json.JsonElement?>(),
                CaseStudySlug = proof?.Slug,
                CaseStudyTitle = proof?.Title,
                PublishedAt = translation.PublishedAt,
                UpdatedDate = translation.UpdatedDate ?? translation.CreatedDate,
                Alternates = AlternatesOf(live),
            };
        }

        /// <summary>
        /// Proof case studies in this language — and only the published ones. A
        /// service page that linked to an unpublished case study would advertise a
        /// 404 as evidence.
        /// </summary>
        private async Task<IReadOnlyDictionary<Guid, CaseStudyTranslation>> LoadProofsAsync(
            IEnumerable<Guid?> caseStudyIds,
            string localeCode
        )
        {
            var ids = caseStudyIds.OfType<Guid>().Distinct().ToList();
            if (ids.Count == 0)
                return new Dictionary<Guid, CaseStudyTranslation>();

            var translations = await _caseStudyTranslations.GetListAsync(q =>
                q.Where(t =>
                    ids.Contains(t.ParentId)
                    && t.LocaleCode == localeCode
                    && t.Status == ContentStatus.Published
                )
            );

            return translations.ToDictionary(t => t.ParentId);
        }
    }
}
