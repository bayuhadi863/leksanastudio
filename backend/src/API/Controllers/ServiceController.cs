using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Service;
using LeksanaStudio.Application.Interfaces.Services;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/service")]
    [MenuCode("service")]
    public class ServiceController
        : BaseTranslatableCrudController<
            IServiceService,
            Domain.Entities.Content.Service,
            ServiceDTO,
            ServiceParam,
            ServicePaginationDTO,
            ServicePaginationParam
        >
    {
        public ServiceController(IServiceService service)
            : base(service) { }
    }
}
