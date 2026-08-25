using LeksanaStudio.Application.DTOs.ProjectPhase;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IProjectPhaseService
        : ITranslatableCrudService<
            ProjectPhase,
            ProjectPhaseDTO,
            ProjectPhaseParam,
            ProjectPhasePaginationDTO,
            ProjectPhasePaginationParam
        >
    {
        /// <summary>Published phases in one language, in sequence.</summary>
        Task<IEnumerable<ProjectPhasePublicDTO>> GetPublicListAsync(string localeCode);
    }
}
