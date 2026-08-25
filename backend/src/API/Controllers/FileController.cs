using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Controllers
{
    [Route("api/v1/file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        [JwtAuthorize]
        [RequestSizeLimit(5 * 1024 * 1024)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Upload(
            IFormFile file,
            [FromQuery] string folder = "news"
        )
        {
            var filePath = await _fileService.UploadFileAsync(file, folder);
            return Ok(BaseResponse<string>.Ok(filePath, "File berhasil diupload"));
        }

        [HttpPost("upload-document")]
        [JwtAuthorize]
        [RequestSizeLimit(20 * 1024 * 1024)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadDocument(
            IFormFile file,
            [FromQuery] string folder = "catalog"
        )
        {
            var filePath = await _fileService.UploadDocumentAsync(file, folder);
            return Ok(BaseResponse<string>.Ok(filePath, "File berhasil diupload"));
        }

        [HttpGet("get-file")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFile([FromQuery] string path)
        {
            var url = await _fileService.GetFileUrlAsync(path);
            return Ok(BaseResponse<string>.Ok(url, "URL file berhasil didapatkan"));
        }

        // Streams file bytes through the backend so the storage URL is never exposed
        // to the client. download=true forces attachment, otherwise inline (viewer).
        [HttpGet("download")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Download(
            [FromQuery] string path,
            [FromQuery] bool download = false,
            [FromQuery] string? fileName = null
        )
        {
            var (stream, contentType, storedName) = await _fileService.DownloadFileAsync(path);

            var downloadName = string.IsNullOrWhiteSpace(fileName) ? storedName : fileName;

            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = downloadName,
                Inline = !download,
            };
            Response.Headers.ContentDisposition = contentDisposition.ToString();

            return File(stream, contentType, enableRangeProcessing: true);
        }
    }
}
