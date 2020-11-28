using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace RedisDemo.WebFramework.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IDistributedCache _cache;

        public ValuesController()
        {
            var redisOptions = new RedisCacheOptions
            {
                Configuration = "localhost"
            };
            _cache = new RedisCache(redisOptions);
        }


        // GET api/values
        public async Task<IHttpActionResult> Get()
        {
            var cacheKey = $"cache-values-{DateTime.UtcNow:yyyyMMdd-HHmm}";

            var stringValue = await _cache.GetStringAsync(cacheKey);

            if(stringValue != null)
            {
                return Ok(JsonConvert.DeserializeObject<string[]>(stringValue));
            }

            var result = new string[] { "value1", "value2" };

            var jsonString = JsonConvert.SerializeObject(result);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
            };
            await _cache.SetStringAsync(cacheKey, jsonString, options);

            return Ok(result);
        }
    }
}
