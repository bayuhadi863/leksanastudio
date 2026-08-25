using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.Vertical;
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
    /// Industry landing pages — the studio's SEO engine (blueprint 08).
    ///
    /// One template, many rows, each written entirely in one industry's language.
    /// They sit at the site root, which is why their addresses have to be unique
    /// against everything else and never assigned casually.
    /// </summary>
    public class VerticalService
        : BaseTranslatableCrudService<
            Vertical,
            VerticalTranslation,
            VerticalDTO,
            VerticalParam,
            VerticalTranslationParam,
            VerticalPaginationDTO,
            VerticalPaginationParam
        >,
            IVerticalService
    {
        private readonly IBaseRepository<ServiceTranslation> _serviceTranslations;

        public VerticalService(
            IBaseRepository<Vertical> repository,
            IBaseRepository<VerticalTranslation> translations,
            IBaseRepository<ServiceTranslation> serviceTranslations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService)
        {
            _serviceTranslations = serviceTranslations;
        }

        protected override string ContentType => "vertical";

        protected override string? FallbackSlugSource(VerticalTranslation translation) =>
            translation.Industry;

        protected override string? DisplayName(VerticalTranslation translation) =>
            translation.Industry;

        /* --------------------------------------------------------------- panel */

        protected override IQueryable<Vertical> ApplyFilter(
            IQueryable<Vertical> query,
            VerticalPaginationParam param
        )
        {
            if (param.ServiceId is { } serviceId)
                query = query.Where(v => v.ServiceId == serviceId);

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(v =>
                    v.Translations.Any(t =>
                        !t.IsDeleted
                        && (
                            (t.Industry != null && EF.Functions.ILike(t.Industry, term))
                            || (t.Headline != null && EF.Functions.ILike(t.Headline, term))
                            || (t.Slug != null && EF.Functions.ILike(t.Slug, term))
                        )
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(Vertical entity, VerticalDTO dto)
        {
            dto.Translations = await LoadTranslationDtosAsync<VerticalTranslationDTO>(entity.Id);

            if (entity.ServiceId is { } serviceId)
            {
                var defaultLocale = await _locales.GetDefaultCodeAsync();
                var service = await _serviceTranslations.GetAsync(q =>
                    q.Where(t => t.ParentId == serviceId && t.LocaleCode == defaultLocale)
                );
                dto.ServiceName = service?.Name;
            }
        }

        /* -------------------------------------------------------------- public */

        public async Task<IEnumerable<VerticalPublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);
            var services = await LoadServicesAsync(entities.Select(v => v.ServiceId), localeCode);

            return entities
                .Select(entity => ToPublic(entity, localeCode, services))
                .OfType<VerticalPublicDTO>();
        }

        public async Task<VerticalPublicDTO?> GetPublicBySlugAsync(string localeCode, string slug)
        {
            var entity = await GetPublishedBySlugAsync(localeCode, slug);
            if (entity is null)
                return null;

            var services = await LoadServicesAsync([entity.ServiceId], localeCode);
            return ToPublic(entity, localeCode, services);
        }

        private static VerticalPublicDTO? ToPublic(
            Vertical entity,
            string localeCode,
            IReadOnlyDictionary<Guid, ServiceTranslation> services
        )
        {
            var live = LiveTranslations(entity);
            var translation = live.FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            var service = entity.ServiceId is { } id ? services.GetValueOrDefault(id) : null;

            return new VerticalPublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Slug = translation.Slug,
                PricingShape = entity.PricingShape,
                Order = entity.Order,
                Industry = translation.Industry,
                Headline = translation.Headline,
                Intro = translation.Intro,
                Note = translation.Note,
                WhatsappIntro = translation.WhatsappIntro,
                Problems = translation.Problems.Adapt<System.Text.Json.JsonElement?>(),
                Deliverables = translation.Deliverables.Adapt<System.Text.Json.JsonElement?>(),
                Faq = translation.Faq.Adapt<System.Text.Json.JsonElement?>(),
                ServiceSlug = service?.Slug,
                ServiceName = service?.Name,
                PublishedAt = translation.PublishedAt,
                UpdatedDate = translation.UpdatedDate ?? translation.CreatedDate,
                Alternates = AlternatesOf(live),
            };
        }

        private async Task<IReadOnlyDictionary<Guid, ServiceTranslation>> LoadServicesAsync(
            IEnumerable<Guid?> serviceIds,
            string localeCode
        )
        {
            var ids = serviceIds.OfType<Guid>().Distinct().ToList();
            if (ids.Count == 0)
                return new Dictionary<Guid, ServiceTranslation>();

            var translations = await _serviceTranslations.GetListAsync(q =>
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
