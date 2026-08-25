using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.ProjectPhase;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/project-phase")]
    [MenuCode("project-phase")]
    public class ProjectPhaseController
        : BaseTranslatableCrudController<
            IProjectPhaseService,
            ProjectPhase,
            ProjectPhaseDTO,
            ProjectPhaseParam,
            ProjectPhasePaginationDTO,
            ProjectPhasePaginationParam
        >
    {
        public ProjectPhaseController(IProjectPhaseService service)
            : base(service) { }
    }
}
