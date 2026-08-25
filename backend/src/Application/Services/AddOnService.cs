using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.AddOn;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// Extra work, priced separately.
    ///
    /// Its own list rather than a column in every package: an add-on is mentioned
    /// once the main need is settled, and a price table that shows everything at
    /// once delays the decision it exists to help.
    /// </summary>
    public class AddOnService
        : BaseTranslatableCrudService<
            AddOn,
            AddOnTranslation,
            AddOnDTO,
            AddOnParam,
            AddOnTranslationParam,
            AddOnPaginationDTO,
            AddOnPaginationParam
        >,
            IAddOnService
    {
        public AddOnService(
            IBaseRepository<AddOn> repository,
            IBaseRepository<AddOnTranslation> translations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService) { }

        protected override string ContentType => "add-on";

        protected override bool SlugRequired => false;

        protected override string? DisplayName(AddOnTranslation translation) => translation.Name;

        protected override IQueryable<AddOn> ApplyFilter(
            IQueryable<AddOn> query,
            AddOnPaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(a =>
                    a.Translations.Any(t =>
                        !t.IsDeleted && t.Name != null && EF.Functions.ILike(t.Name, term)
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(AddOn entity, AddOnDTO dto)
        {
            dto.Translations = await LoadTranslationDtosAsync<AddOnTranslationDTO>(entity.Id);
        }

        public async Task<IEnumerable<AddOnPublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);
            return entities.Select(entity => ToPublic(entity, localeCode)).OfType<AddOnPublicDTO>();
        }

        private static AddOnPublicDTO? ToPublic(AddOn entity, string localeCode)
        {
            var translation = LiveTranslations(entity)
                .FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            return new AddOnPublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Order = entity.Order,
                Name = translation.Name,
                Price = translation.Price,
                AppliesTo = translation.AppliesTo,
            };
        }
    }
}
