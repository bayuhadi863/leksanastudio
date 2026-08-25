using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Media;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/media")]
    [MenuCode("media")]
    public class MediaController
        : BaseCrudController<
            IMediaService,
            Domain.Entities.Media,
            MediaDTO,
            MediaParam,
            MediaPaginationDTO,
            MediaPaginationParam
        >
    {
        public MediaController(IMediaService service)
            : base(service) { }

        /// <summary>
        /// Uploads one image and records it.
        ///
        /// Kept separate from <c>create</c> because a file upload is multipart and a
        /// content write is JSON — folding them together would make both worse.
        /// </summary>
        [HttpPost("upload")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.Create)]
        [RequestSizeLimit(5 * 1024 * 1024)]
        [ProducesResponseType(typeof(BaseResponse<MediaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] MediaUploadParam param)
        {
            var result = await _service.UploadAsync(file, param);
            return Ok(BaseResponse<MediaDTO>.Ok(result, "Berkas berhasil diunggah"));
        }
    }
}
