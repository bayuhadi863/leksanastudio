using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Vertical;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/vertical")]
    [MenuCode("vertical")]
    public class VerticalController
        : BaseTranslatableCrudController<
            IVerticalService,
            Vertical,
            VerticalDTO,
            VerticalParam,
            VerticalPaginationDTO,
            VerticalPaginationParam
        >
    {
        public VerticalController(IVerticalService service)
            : base(service) { }
    }
}
