using Microsoft.Extensions.Options;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Infrastructure.Options;

namespace LeksanaStudio.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IStorageService _storageService;
        private readonly MinioOptions _options;
        private readonly FileUploadOptions _uploadOptions;

        private readonly HashSet<string> _allowedContentTypes;
        private readonly HashSet<string> _allowedDocumentContentTypes;

        // Report templates may be a fillable Word doc or a PDF. Validated by
        // extension since browsers are inconsistent about the .docx content type.
        private readonly HashSet<string> _allowedTemplateExtensions;

        public FileService(
            IStorageService storageService,
            IOptions<MinioOptions> options,
            IOptions<FileUploadOptions> uploadOptions
        )
        {
            _storageService = storageService;
            _options = options.Value;
            _uploadOptions = uploadOptions.Value;

            _allowedContentTypes = new(
                _uploadOptions.AllowedImageContentTypes,
                StringComparer.OrdinalIgnoreCase
            );
            _allowedDocumentContentTypes = new(
                _uploadOptions.AllowedDocumentContentTypes,
                StringComparer.OrdinalIgnoreCase
            );
            _allowedTemplateExtensions = new(
                _uploadOptions.AllowedTemplateExtensions,
                StringComparer.OrdinalIgnoreCase
            );
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            if (file.Length == 0)
                throw new BadRequestException("File tidak boleh kosong");

            if (file.Length > _uploadOptions.MaxImageSizeBytes)
                throw new BadRequestException(
                    $"Ukuran file maksimal {_uploadOptions.MaxImageSizeBytes / 1024 / 1024}MB"
                );

            if (!_allowedContentTypes.Contains(file.ContentType))
                throw new BadRequestException(
                    "Tipe file tidak didukung. Gunakan JPEG, PNG, GIF, atau WebP"
                );

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var objectName =
                $"{folder}/{DateTimeOffset.UtcNow:yyyy-MM}/{Guid.CreateVersion7()}{extension}";

            await using var stream = file.OpenReadStream();
            return await _storageService.UploadAsync(
                stream,
                objectName,
                file.ContentType,
                _options.BucketName
            );
        }

        public async Task<string> UploadDocumentAsync(IFormFile file, string folder)
        {
            if (file.Length == 0)
                throw new BadRequestException("File tidak boleh kosong");

            if (file.Length > _uploadOptions.MaxDocumentSizeBytes)
                throw new BadRequestException(
                    $"Ukuran file maksimal {_uploadOptions.MaxDocumentSizeBytes / 1024 / 1024}MB"
                );

            if (!_allowedDocumentContentTypes.Contains(file.ContentType))
                throw new BadRequestException("Tipe file tidak didukung. Gunakan file PDF");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var objectName =
                $"{folder}/{DateTimeOffset.UtcNow:yyyy-MM}/{Guid.CreateVersion7()}{extension}";

            await using var stream = file.OpenReadStream();
            return await _storageService.UploadAsync(
                stream,
                objectName,
                file.ContentType,
                _options.BucketName
            );
        }

        public async Task<string> UploadTemplateDocumentAsync(IFormFile file, string folder)
        {
            if (file.Length == 0)
                throw new BadRequestException("File tidak boleh kosong");

            if (file.Length > _uploadOptions.MaxDocumentSizeBytes)
                throw new BadRequestException(
                    $"Ukuran file maksimal {_uploadOptions.MaxDocumentSizeBytes / 1024 / 1024}MB"
                );

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedTemplateExtensions.Contains(extension))
                throw new BadRequestException(
                    "Tipe file tidak didukung. Gunakan PDF atau DOCX"
                );

            var objectName =
                $"{folder}/{DateTimeOffset.UtcNow:yyyy-MM}/{Guid.CreateVersion7()}{extension}";

            await using var stream = file.OpenReadStream();
            return await _storageService.UploadAsync(
                stream,
                objectName,
                file.ContentType,
                _options.BucketName
            );
        }

        public async Task<string> GetFileUrlAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new BadRequestException("Path file tidak boleh kosong");

            var separatorIndex = filePath.IndexOf('/');
            if (separatorIndex < 0)
                throw new BadRequestException("Format path file tidak valid");

            var bucketName = filePath[..separatorIndex];
            var objectName = filePath[(separatorIndex + 1)..];

            return await _storageService.GetPresignedUrlAsync(objectName, bucketName);
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> DownloadFileAsync(
            string filePath
        )
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new BadRequestException("Path file tidak boleh kosong");

            var separatorIndex = filePath.IndexOf('/');
            if (separatorIndex < 0)
                throw new BadRequestException("Format path file tidak valid");

            var bucketName = filePath[..separatorIndex];
            var objectName = filePath[(separatorIndex + 1)..];

            var (stream, contentType) = await _storageService.GetObjectAsync(
                objectName,
                bucketName
            );
            var fileName = Path.GetFileName(objectName);

            return (stream, contentType, fileName);
        }
    }
}
