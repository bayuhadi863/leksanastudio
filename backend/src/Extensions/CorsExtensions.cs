using LeksanaStudio.Infrastructure.Options;

namespace LeksanaStudio.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsPolicy(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var options =
                configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>()
                ?? new CorsOptions();

            services.AddCors(o =>
            {
                o.AddDefaultPolicy(builder =>
                {
                    // No configured origins => permissive (local/dev). Configure
                    // Cors:AllowedOrigins in production to lock this down.
                    if (options.AllowedOrigins.Length == 0)
                        builder.AllowAnyOrigin();
                    else
                        builder.WithOrigins(options.AllowedOrigins).AllowCredentials();

                    builder.AllowAnyMethod().AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
