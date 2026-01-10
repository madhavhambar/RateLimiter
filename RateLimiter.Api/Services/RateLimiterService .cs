using Microsoft.Extensions.Options;
using RateLimiter.Api.Interfaces;
using RateLimiter.Api.Options;

namespace RateLimiter.Api.Services
{
    public class RateLimiterService : IRateLimiterService
    {
        private readonly IRateLimitStore store;
        private readonly IOptionsMonitor<RateLimitOptions> options;

        public RateLimiterService(
            IRateLimitStore store,
            IOptionsMonitor<RateLimitOptions> options)
        {
            this.store = store;
            this.options = options;
        }

        public bool IsAllowed(string identifier)
        {
            var rule = options.CurrentValue;
            var now = DateTime.UtcNow;

            var windowStartTicks =
                now.Ticks - (now.Ticks % TimeSpan.FromSeconds(rule.WindowSeconds).Ticks);

            var windowStart = new DateTime(windowStartTicks, DateTimeKind.Utc);

            var count = store.Increment(identifier, windowStart);
            return count <= rule.Requests;
        }
    }
}
