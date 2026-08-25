using LeksanaStudio.Application.DTOs.AddOn;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IAddOnService
        : ITranslatableCrudService<
            AddOn,
            AddOnDTO,
            AddOnParam,
            AddOnPaginationDTO,
            AddOnPaginationParam
        >
    {
        /// <summary>Published add-ons in one language, in display order.</summary>
        Task<IEnumerable<AddOnPublicDTO>> GetPublicListAsync(string localeCode);
    }
}
