using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Note;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/note")]
    [MenuCode("note")]
    public class NoteController
        : BaseTranslatableCrudController<
            INoteService,
            Note,
            NoteDTO,
            NoteParam,
            NotePaginationDTO,
            NotePaginationParam
        >
    {
        public NoteController(INoteService service)
            : base(service) { }
    }
}
