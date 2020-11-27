using Microsoft.Extensions.Caching.Distributed;
using RedisDemo.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RedisDemo.Web.Data
{
    public interface IDashboardDataService
    {
        Task<IEnumerable<string>> GetStatsAsync();
    }

    public class DashboardDataService : IDashboardDataService
    {
        private readonly IDistributedCache _cache;

        public DashboardDataService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<string>> GetStatsAsync()
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
