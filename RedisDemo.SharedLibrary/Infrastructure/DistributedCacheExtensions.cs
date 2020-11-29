using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace RedisDemo.SharedLibrary.Infrastructure
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetItem<T>(
            this IDistributedCache cache,
            string cacheKey,
            T item,
            TimeSpan? absoluteExpiration = null,
            TimeSpan? slidingExpiration = null)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(absoluteExpiration ?? TimeSpan.FromSeconds(60));

            if (slidingExpiration != null)
                options.SetSlidingExpiration(slidingExpiration.Value);

            await cache.SetStringAsync(cacheKey, jsonString, options);
        }


        public static async Task<T> GetValue<T>(this IDistributedCache cache, string cacheKey)
        {
            var stringValue = await cache.GetStringAsync(cacheKey);
            if (stringValue == null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(stringValue);
        }
    }
}
