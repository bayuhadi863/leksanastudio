using LeksanaStudio.Application.DTOs.Locale;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface ILocaleService
        : IBaseCrudService<
            Domain.Entities.Locale,
            LocaleDTO,
            LocaleParam,
            LocalePaginationDTO,
            LocalePaginationParam
        >
    {
        /// <summary>The languages the public site may serve, in display order.</summary>
        Task<IEnumerable<LocaleDTO>> GetActiveAsync();

        /// <summary>
        /// The code every translation falls back to when none is asked for.
        /// Cached for the request, since almost every content query needs it.
        /// </summary>
        Task<string> GetDefaultCodeAsync();
    }
}
