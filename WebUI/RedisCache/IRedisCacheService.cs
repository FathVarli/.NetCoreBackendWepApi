using System;
using System.Threading.Tasks;

namespace WebUI.RedisCache
{
    public interface IRedisCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeTimeLive);

        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
