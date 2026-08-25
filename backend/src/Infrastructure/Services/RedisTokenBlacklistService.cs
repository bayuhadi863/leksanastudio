using LeksanaStudio.Common.Interfaces;
using StackExchange.Redis;

namespace LeksanaStudio.Infrastructure.Services
{
    public class RedisTokenBlacklistService : ITokenBlacklistService
    {
        private readonly IConnectionMultiplexer _redis;
        private const string KeyPrefix = "blacklist:token:";

        public RedisTokenBlacklistService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task BlacklistAsync(string jti, TimeSpan expiry)
        {
            try
            {
                var db = _redis.GetDatabase();
                await db.StringSetAsync($"{KeyPrefix}{jti}", "1", expiry);
            }
            catch
            {
                // Fail silently — token not blacklisted if Redis is unavailable
            }
        }

        public async Task<bool> IsBlacklistedAsync(string jti)
        {
            try
            {
                var db = _redis.GetDatabase();
                return await db.KeyExistsAsync($"{KeyPrefix}{jti}");
            }
            catch
            {
                // Fail open — allow request if Redis is unavailable
                return false;
            }
        }
    }
}
