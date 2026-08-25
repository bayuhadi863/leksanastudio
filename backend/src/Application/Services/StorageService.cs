using Minio;
using Minio.DataModel.Args;
using LeksanaStudio.Application.Interfaces.Services;

namespace LeksanaStudio.Application.Services
{
    public class StorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;

        public StorageService(IMinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        public async Task EnsureBucketExistsAsync(string bucketName)
        {
            var exists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName)
            );

            if (!exists)
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(bucketName)
                );
            }
        }

        public async Task<string> UploadAsync(
            Stream stream,
            string objectName,
            string contentType,
            string bucketName
        )
        {
            await EnsureBucketExistsAsync(bucketName);

            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(args);

            return $"{bucketName}/{objectName}";
        }

        public async Task<string> GetPresignedUrlAsync(
            string objectName,
            string bucketName,
            int expirySeconds = 3600
        )
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(expirySeconds);

            return await _minioClient.PresignedGetObjectAsync(args);
        }

        public async Task<(Stream Stream, string ContentType)> GetObjectAsync(
            string objectName,
            string bucketName
        )
        {
            var stat = await _minioClient.StatObjectAsync(
                new StatObjectArgs().WithBucket(bucketName).WithObject(objectName)
            );

            var memoryStream = new MemoryStream();
            await _minioClient.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(
                        async (stream, cancellationToken) =>
                            await stream.CopyToAsync(memoryStream, cancellationToken)
                    )
            );

            memoryStream.Position = 0;
            return (memoryStream, stat.ContentType);
        }
    }
}
