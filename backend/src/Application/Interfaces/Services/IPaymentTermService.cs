using LeksanaStudio.Application.DTOs.PaymentTerm;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IPaymentTermService
        : ITranslatableCrudService<
            PaymentTerm,
            PaymentTermDTO,
            PaymentTermParam,
            PaymentTermPaginationDTO,
            PaymentTermPaginationParam
        >
    {
        /// <summary>Published payment terms in one language, in display order.</summary>
        Task<IEnumerable<PaymentTermPublicDTO>> GetPublicListAsync(string localeCode);
    }
}
