using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Website.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetAsync<T>(this IDistributedCache cache,
            string key,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
                SlidingExpiration = unusedExpireTime
            };

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(key, jsonData, options);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var jsonData = await cache.GetStringAsync(key);

            if (jsonData is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

       
    }
}
