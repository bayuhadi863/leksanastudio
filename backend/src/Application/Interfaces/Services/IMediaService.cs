using LeksanaStudio.Application.DTOs.Media;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IMediaService
        : IBaseCrudService<
            Domain.Entities.Media,
            MediaDTO,
            MediaParam,
            MediaPaginationDTO,
            MediaPaginationParam
        >
    {
        /// <summary>Stores the file and records it, returning the entry the panel then references.</summary>
        Task<MediaDTO> UploadAsync(IFormFile file, MediaUploadParam param);
    }
}
