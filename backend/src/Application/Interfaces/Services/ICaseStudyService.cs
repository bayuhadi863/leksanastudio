using LeksanaStudio.Application.DTOs.CaseStudy;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface ICaseStudyService
        : ITranslatableCrudService<
            CaseStudy,
            CaseStudyDTO,
            CaseStudyParam,
            CaseStudyPaginationDTO,
            CaseStudyPaginationParam
        >
    {
        /// <summary>Published case studies in one language, in display order.</summary>
        Task<IEnumerable<CaseStudyPublicDTO>> GetPublicListAsync(string localeCode);

        /// <summary>One published case study by its address, or null.</summary>
        Task<CaseStudyPublicDTO?> GetPublicBySlugAsync(string localeCode, string slug);
    }
}
