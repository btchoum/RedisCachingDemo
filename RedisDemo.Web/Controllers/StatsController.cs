using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RedisDemo.Web.Infrastructure;

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

            var result = await _cache.GetValue<IEnumerable<string>>(cacheKey);
            if (result != null)
            {
                return result;
            }

            var stats = GetStatsFromDatabase();

            await _cache.SetItem(cacheKey, stats, TimeSpan.FromSeconds(60));

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
