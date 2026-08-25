using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.ServicePackage;
using LeksanaStudio.Application.Interfaces.Services;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/service-package")]
    [MenuCode("service-package")]
    public class ServicePackageController
        : BaseTranslatableCrudController<
            IServicePackageService,
            Domain.Entities.Content.ServicePackage,
            ServicePackageDTO,
            ServicePackageParam,
            ServicePackagePaginationDTO,
            ServicePackagePaginationParam
        >
    {
        public ServicePackageController(IServicePackageService service)
            : base(service) { }
    }
}
