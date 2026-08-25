using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.PaymentTerm;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/payment-term")]
    [MenuCode("payment-term")]
    public class PaymentTermController
        : BaseTranslatableCrudController<
            IPaymentTermService,
            PaymentTerm,
            PaymentTermDTO,
            PaymentTermParam,
            PaymentTermPaginationDTO,
            PaymentTermPaginationParam
        >
    {
        public PaymentTermController(IPaymentTermService service)
            : base(service) { }
    }
}
