using System;

namespace RedisDemo.SharedLibrary.Data
{
    public class SummaryToday
    {
        public int UserLogins { get; set; }
        public decimal Sales { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
