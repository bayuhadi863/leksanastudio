using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.ProcessStep;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/process-step")]
    [MenuCode("process-step")]
    public class ProcessStepController
        : BaseTranslatableCrudController<
            IProcessStepService,
            ProcessStep,
            ProcessStepDTO,
            ProcessStepParam,
            ProcessStepPaginationDTO,
            ProcessStepPaginationParam
        >
    {
        public ProcessStepController(IProcessStepService service)
            : base(service) { }
    }
}
