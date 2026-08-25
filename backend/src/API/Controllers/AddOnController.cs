using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.AddOn;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/add-on")]
    [MenuCode("add-on")]
    public class AddOnController
        : BaseTranslatableCrudController<
            IAddOnService,
            AddOn,
            AddOnDTO,
            AddOnParam,
            AddOnPaginationDTO,
            AddOnPaginationParam
        >
    {
        public AddOnController(IAddOnService service)
            : base(service) { }
    }
}
