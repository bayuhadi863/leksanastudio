using LeksanaStudio.Application.DTOs.ProcessStep;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IProcessStepService
        : ITranslatableCrudService<
            ProcessStep,
            ProcessStepDTO,
            ProcessStepParam,
            ProcessStepPaginationDTO,
            ProcessStepPaginationParam
        >
    {
        /// <summary>Published process steps in one language, in sequence.</summary>
        Task<IEnumerable<ProcessStepPublicDTO>> GetPublicListAsync(string localeCode);
    }
}
