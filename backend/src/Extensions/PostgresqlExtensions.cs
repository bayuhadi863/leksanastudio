using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LeksanaStudio.Infrastructure.DataContext;
using LeksanaStudio.Infrastructure.Options;

namespace LeksanaStudio.Extensions
{
    public static class PostgresqlExtensions
    {
        public static IServiceCollection AddPostgresql(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<PostgresqlOptions>(
                configuration.GetSection(PostgresqlOptions.SectionName)
            );

            services.AddDbContext<AppDbContext>(
                (serviceProvider, options) =>
                {
                    var postgresqlOptions = serviceProvider
                        .GetRequiredService<IOptions<PostgresqlOptions>>()
                        .Value;
                    options.UseNpgsql(postgresqlOptions.ConnectionString);
                }
            );

            return services;
        }
    }
}
