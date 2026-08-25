using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.PageDocument;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/page-document")]
    [MenuCode("page-document")]
    public class PageDocumentController
        : BaseTranslatableCrudController<
            IPageDocumentService,
            PageDocument,
            PageDocumentDTO,
            PageDocumentParam,
            PageDocumentPaginationDTO,
            PageDocumentPaginationParam
        >
    {
        public PageDocumentController(IPageDocumentService service)
            : base(service) { }
    }
}
