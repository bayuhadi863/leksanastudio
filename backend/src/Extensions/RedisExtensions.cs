using Microsoft.Extensions.Options;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Infrastructure.Options;
using LeksanaStudio.Infrastructure.Services;
using StackExchange.Redis;

namespace LeksanaStudio.Extensions
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedis(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<RedisOptions>(
                configuration.GetSection(RedisOptions.SectionName)
            );

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
                return ConnectionMultiplexer.Connect(options.ConnectionString);
            });

            services.AddScoped<ITokenBlacklistService, RedisTokenBlacklistService>();

            return services;
        }
    }
}
