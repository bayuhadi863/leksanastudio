using LeksanaStudio.Application.DTOs.Note;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface INoteService
        : ITranslatableCrudService<
            Note,
            NoteDTO,
            NoteParam,
            NotePaginationDTO,
            NotePaginationParam
        >
    {
        /// <summary>Published notes in one language, newest editorial order first.</summary>
        Task<IEnumerable<NotePublicDTO>> GetPublicListAsync(string localeCode);

        /// <summary>One published note by its address, or null.</summary>
        Task<NotePublicDTO?> GetPublicBySlugAsync(string localeCode, string slug);
    }
}
