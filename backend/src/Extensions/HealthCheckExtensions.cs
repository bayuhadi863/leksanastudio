using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using LeksanaStudio.Application.Seeders;
using LeksanaStudio.Infrastructure.HealthChecks;
using LeksanaStudio.Infrastructure.Options;
using StackExchange.Redis;

namespace LeksanaStudio.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthChecksServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddHealthChecks()
                .AddNpgSql(
                    sp =>
                        sp.GetRequiredService<IOptions<PostgresqlOptions>>().Value.ConnectionString,
                    name: "postgresql",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: ["db", "postgresql"]
                )
                .AddRedis(
                    sp => sp.GetRequiredService<IConnectionMultiplexer>(),
                    name: "redis",
                    failureStatus: HealthStatus.Degraded,
                    tags: ["cache", "redis"]
                )
                .AddCheck<MinioHealthCheck>(
                    "minio",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: ["storage", "minio"]
                );

            return services;
        }

        public static WebApplication UseHealthChecksEndpoint(this WebApplication app)
        {
            app.MapHealthChecks(
                "/health",
                new HealthCheckOptions
                {
                    ResponseWriter = WriteHealthCheckResponse,
                    AllowCachingResponses = false,
                }
            );

            return app;
        }

        public static async Task RunStartupChecksAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var healthCheckService = scope.ServiceProvider.GetRequiredService<HealthCheckService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Running startup health checks...");

            var result = await healthCheckService.CheckHealthAsync();

            foreach (var (name, entry) in result.Entries)
            {
                var level = entry.Status switch
                {
                    HealthStatus.Healthy => LogLevel.Information,
                    HealthStatus.Degraded => LogLevel.Warning,
                    _ => LogLevel.Error,
                };

                logger.Log(
                    level,
                    "Health check [{Name}]: {Status} — {Description}",
                    name,
                    entry.Status,
                    entry.Description ?? "OK"
                );
            }

            if (result.Status == HealthStatus.Unhealthy)
            {
                logger.LogCritical(
                    "Critical health checks failed at startup. Status: {Status}",
                    result.Status
                );
                throw new InvalidOperationException(
                    "Application startup failed: one or more critical health checks are unhealthy."
                );
            }

            if (result.Status == HealthStatus.Degraded)
            {
                logger.LogWarning(
                    "Some non-critical services are degraded. Application will continue."
                );
            }

            logger.LogInformation("Startup health checks passed. Seeding database...");
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAllAsync();
        }

        private static async Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
        {
            context.Response.StatusCode =
                report.Status == HealthStatus.Healthy
                    ? StatusCodes.Status200OK
                    : StatusCodes.Status503ServiceUnavailable;

            var response = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = $"{e.Value.Duration.TotalMilliseconds:F2}ms",
                    tags = e.Value.Tags,
                    error = e.Value.Exception?.Message,
                }),
                totalDuration = $"{report.TotalDuration.TotalMilliseconds:F2}ms",
            };

            await context.Response.WriteAsJsonAsync(
                response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
        }
    }
}
