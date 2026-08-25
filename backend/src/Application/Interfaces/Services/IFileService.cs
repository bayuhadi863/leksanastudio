namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
        Task<string> UploadDocumentAsync(IFormFile file, string folder);
        Task<string> UploadTemplateDocumentAsync(IFormFile file, string folder);
        Task<string> GetFileUrlAsync(string filePath);
        Task<(Stream Stream, string ContentType, string FileName)> DownloadFileAsync(
            string filePath
        );
    }
}
