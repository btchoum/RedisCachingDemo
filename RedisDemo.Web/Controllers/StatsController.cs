using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisDemo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public StatsController(IDistributedCache cache)
        {
            _cache = cache;
        }

        // GET api/stats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            var result = await GetStats();

            return Ok(result);
        }

        private async Task<IEnumerable<string>> GetStats()
        {
            string cacheKey = $"redis/demo/values{DateTime.UtcNow:yyyyMMdd_HHmm}";

            var stringValue = await _cache.GetStringAsync(cacheKey);
            if (stringValue != null)
            {
                return JsonConvert.DeserializeObject<string[]>(stringValue);
            }

            var stats = GetStatsFromDatabase();

            var jsonString = JsonConvert.SerializeObject(stats);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60));

            await _cache.SetStringAsync(cacheKey, jsonString, options);

            return stats;
        }

        private static IEnumerable<string> GetStatsFromDatabase()
        {
            Thread.Sleep(1000);

            return new[]
            {
                "stat 1",
                "stat 2",
                "stat 3",
            };
        }
    }
}
