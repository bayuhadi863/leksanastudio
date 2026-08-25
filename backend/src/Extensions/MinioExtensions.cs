using Microsoft.Extensions.Options;
using Minio;
using LeksanaStudio.Infrastructure.Options;

namespace LeksanaStudio.Extensions
{
    public static class MinioExtensions
    {
        public static IServiceCollection AddMinioStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.SectionName));
            services.Configure<FileUploadOptions>(
                configuration.GetSection(FileUploadOptions.SectionName)
            );

            services.AddSingleton<IMinioClient>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MinioOptions>>().Value;
                return new MinioClient()
                    .WithEndpoint(options.Endpoint, options.Port)
                    .WithCredentials(options.AccessKey, options.SecretKey)
                    .WithSSL(options.UseSSL)
                    .Build();
            });

            return services;
        }
    }
}
