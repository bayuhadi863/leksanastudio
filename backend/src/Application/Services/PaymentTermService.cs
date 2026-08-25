using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.PaymentTerm;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// How payment is staged, per kind of work.
    ///
    /// Published on the site rather than kept for the proposal: a prospect who can
    /// read the terms before asking is a prospect who does not have to trust a
    /// stranger's word about them.
    /// </summary>
    public class PaymentTermService
        : BaseTranslatableCrudService<
            PaymentTerm,
            PaymentTermTranslation,
            PaymentTermDTO,
            PaymentTermParam,
            PaymentTermTranslationParam,
            PaymentTermPaginationDTO,
            PaymentTermPaginationParam
        >,
            IPaymentTermService
    {
        public PaymentTermService(
            IBaseRepository<PaymentTerm> repository,
            IBaseRepository<PaymentTermTranslation> translations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService) { }

        protected override string ContentType => "payment-term";

        protected override bool SlugRequired => false;

        protected override string? DisplayName(PaymentTermTranslation translation) =>
            translation.Scope;

        protected override IQueryable<PaymentTerm> ApplyFilter(
            IQueryable<PaymentTerm> query,
            PaymentTermPaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(p =>
                    p.Translations.Any(t =>
                        !t.IsDeleted && t.Scope != null && EF.Functions.ILike(t.Scope, term)
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(PaymentTerm entity, PaymentTermDTO dto)
        {
            dto.Translations = await LoadTranslationDtosAsync<PaymentTermTranslationDTO>(entity.Id);
        }

        public async Task<IEnumerable<PaymentTermPublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);
            return entities
                .Select(entity => ToPublic(entity, localeCode))
                .OfType<PaymentTermPublicDTO>();
        }

        private static PaymentTermPublicDTO? ToPublic(PaymentTerm entity, string localeCode)
        {
            var translation = LiveTranslations(entity)
                .FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            return new PaymentTermPublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Order = entity.Order,
                Scope = translation.Scope,
                Schedule = translation.Schedule,
            };
        }
    }
}
