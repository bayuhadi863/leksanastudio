using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using LeksanaStudio.Infrastructure.Options;

namespace LeksanaStudio.Infrastructure.HealthChecks
{
    public class MinioHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MinioOptions _options;

        public MinioHealthCheck(
            IHttpClientFactory httpClientFactory,
            IOptions<MinioOptions> options
        )
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var scheme = _options.UseSSL ? "https" : "http";
                var url = $"{scheme}://{_options.Endpoint}:{_options.Port}/minio/health/live";

                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(5);

                var response = await client.GetAsync(url, cancellationToken);

                return response.IsSuccessStatusCode
                    ? HealthCheckResult.Healthy("MinIO storage is reachable.")
                    : HealthCheckResult.Unhealthy(
                        $"MinIO health endpoint returned {(int)response.StatusCode}."
                    );
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("MinIO storage is unreachable.", ex);
            }
        }
    }
}
