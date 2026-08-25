using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.ServicePackage;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// Columns of a pricing table.
    ///
    /// Packages have no address of their own: they are read inside a comparison,
    /// never on a page of their own, so no slug is minted for them.
    /// </summary>
    public class ServicePackageService
        : BaseTranslatableCrudService<
            Domain.Entities.Content.ServicePackage,
            ServicePackageTranslation,
            ServicePackageDTO,
            ServicePackageParam,
            ServicePackageTranslationParam,
            ServicePackagePaginationDTO,
            ServicePackagePaginationParam
        >,
            IServicePackageService
    {
        public ServicePackageService(
            IBaseRepository<Domain.Entities.Content.ServicePackage> repository,
            IBaseRepository<ServicePackageTranslation> translations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService) { }

        protected override string ContentType => "service-package";

        protected override bool SlugRequired => false;

        protected override string? DisplayName(ServicePackageTranslation translation) =>
            translation.Name;

        /* --------------------------------------------------------------- panel */

        protected override IQueryable<Domain.Entities.Content.ServicePackage> ApplyFilter(
            IQueryable<Domain.Entities.Content.ServicePackage> query,
            ServicePackagePaginationParam param
        )
        {
            if (param.Group is { } group)
                query = query.Where(p => p.Group == group);

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(p =>
                    (p.Code != null && EF.Functions.ILike(p.Code, term))
                    || p.Translations.Any(t =>
                        !t.IsDeleted && t.Name != null && EF.Functions.ILike(t.Name, term)
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(
            Domain.Entities.Content.ServicePackage entity,
            ServicePackageDTO dto
        )
        {
            dto.Translations = await LoadTranslationDtosAsync<ServicePackageTranslationDTO>(
                entity.Id
            );
        }

        /* ------------------------------------------------------ one highlight */

        public override async Task<Guid> CreateAsync(ServicePackageParam param)
        {
            var id = await base.CreateAsync(param);
            if (param.Highlighted)
                await ClearOtherHighlightsAsync(param.Group, id);
            return id;
        }

        public override async Task<Guid> UpdateAsync(Guid id, ServicePackageParam param)
        {
            var result = await base.UpdateAsync(id, param);
            if (param.Highlighted)
                await ClearOtherHighlightsAsync(param.Group, id);
            return result;
        }

        /// <summary>
        /// At most one highlighted package per table.
        ///
        /// Enforced on write rather than trusted to the editor: two highlighted
        /// columns is the same as none — nobody scrolls three cards to find the one
        /// they should have seen first.
        /// </summary>
        private async Task ClearOtherHighlightsAsync(
            Common.Enums.PackageGroup group,
            Guid keepId
        )
        {
            var others = (
                await _repository.GetListAsync(q =>
                    q.Where(p => p.Group == group && p.Highlighted && p.Id != keepId)
                )
            ).ToList();

            if (others.Count == 0)
                return;

            foreach (var other in others)
                other.Highlighted = false;

            await _repository.UpdateRangeAsync(others);
        }

        /* -------------------------------------------------------------- public */

        public async Task<IEnumerable<ServicePackagePublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);

            return entities
                .OrderBy(p => p.Group)
                .ThenBy(p => p.Order)
                .Select(entity => ToPublic(entity, localeCode))
                .OfType<ServicePackagePublicDTO>();
        }

        private static ServicePackagePublicDTO? ToPublic(
            Domain.Entities.Content.ServicePackage entity,
            string localeCode
        )
        {
            var translation = LiveTranslations(entity).FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            return new ServicePackagePublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Group = entity.Group,
                Code = entity.Code,
                Price = entity.Price,
                Highlighted = entity.Highlighted,
                Order = entity.Order,
                Name = translation.Name,
                Audience = translation.Audience,
                Summary = translation.Summary,
                PriceNote = translation.PriceNote,
                Duration = translation.Duration,
                Features = translation.Features.Adapt<System.Text.Json.JsonElement?>(),
            };
        }
    }
}
