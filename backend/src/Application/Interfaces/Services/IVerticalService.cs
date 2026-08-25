using LeksanaStudio.Application.DTOs.Vertical;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IVerticalService
        : ITranslatableCrudService<
            Vertical,
            VerticalDTO,
            VerticalParam,
            VerticalPaginationDTO,
            VerticalPaginationParam
        >
    {
        Task<IEnumerable<VerticalPublicDTO>> GetPublicListAsync(string localeCode);

        Task<VerticalPublicDTO?> GetPublicBySlugAsync(string localeCode, string slug);
    }
}
