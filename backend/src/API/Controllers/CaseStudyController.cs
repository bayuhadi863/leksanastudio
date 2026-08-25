using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.CaseStudy;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/case-study")]
    [MenuCode("case-study")]
    public class CaseStudyController
        : BaseTranslatableCrudController<
            ICaseStudyService,
            CaseStudy,
            CaseStudyDTO,
            CaseStudyParam,
            CaseStudyPaginationDTO,
            CaseStudyPaginationParam
        >
    {
        public CaseStudyController(ICaseStudyService service)
            : base(service) { }
    }
}
