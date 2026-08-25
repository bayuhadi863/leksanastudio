using LeksanaStudio.Application.DTOs.Service;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Application.Interfaces.Services
{
    /// <summary>The three lines of work the studio sells. Named for the entity, not for grammar.</summary>
    public interface IServiceService
        : ITranslatableCrudService<
            Domain.Entities.Content.Service,
            ServiceDTO,
            ServiceParam,
            ServicePaginationDTO,
            ServicePaginationParam
        >
    {
        Task<IEnumerable<ServicePublicDTO>> GetPublicListAsync(string localeCode);

        Task<ServicePublicDTO?> GetPublicBySlugAsync(string localeCode, string slug);
    }
}
