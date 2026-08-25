using LeksanaStudio.Application.DTOs.ServicePackage;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IServicePackageService
        : ITranslatableCrudService<
            Domain.Entities.Content.ServicePackage,
            ServicePackageDTO,
            ServicePackageParam,
            ServicePackagePaginationDTO,
            ServicePackagePaginationParam
        >
    {
        /// <summary>Published packages in one language, in table order.</summary>
        Task<IEnumerable<ServicePackagePublicDTO>> GetPublicListAsync(string localeCode);
    }
}
