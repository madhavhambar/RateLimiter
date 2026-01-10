using RateLimiter.Api.Interfaces;
using System.Collections.Concurrent;

namespace RateLimiter.Api.Stores
{
    public class InMemoryRateLimitStore : IRateLimitStore
    {
        private readonly ConcurrentDictionary<string, int> counter = new();

        public int Increment(string identifier, DateTime windowStart)
        {
            var key = $"{identifier}:{windowStart:yyyyMMddHHmmss}";
            return counter.AddOrUpdate(key, 1, (_, current) => current + 1);
        }
    }
}
