namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IStorageService
    {
        Task<string> UploadAsync(
            Stream stream,
            string objectName,
            string contentType,
            string bucketName
        );
        Task<string> GetPresignedUrlAsync(
            string objectName,
            string bucketName,
            int expirySeconds = 3600
        );
        Task<(Stream Stream, string ContentType)> GetObjectAsync(
            string objectName,
            string bucketName
        );
        Task EnsureBucketExistsAsync(string bucketName);
    }
}
