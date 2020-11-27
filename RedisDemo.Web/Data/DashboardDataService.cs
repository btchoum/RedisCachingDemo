using Microsoft.Extensions.Caching.Distributed;
using RedisDemo.Web.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RedisDemo.Web.Data
{
    public interface IDashboardDataService
    {
        Task<SummaryToday> GetStatsAsync();
    }

    public class DashboardDataService : IDashboardDataService
    {
        private readonly IDistributedCache _cache;

        public DashboardDataService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<SummaryToday> GetStatsAsync()
        {
            string cacheKey = $"daily_stats_{DateTime.UtcNow:yyyyMMdd_HHmm}";

            var result = await _cache.GetValue<SummaryToday>(cacheKey);
            if (result != null)
            {
                return result;
            }

            var stats = GetStatsFromDatabase();

            await _cache.SetItem(cacheKey, stats, TimeSpan.FromSeconds(60));

            return stats;
        }

        private static SummaryToday GetStatsFromDatabase()
        {
            Thread.Sleep(1000);

            var random = new Random();

            return new SummaryToday
            {
                Sales = random.Next(10000),
                UserLogins = random.Next(10000),
                LastUpdated = DateTime.UtcNow
            };
        }
    }
}
