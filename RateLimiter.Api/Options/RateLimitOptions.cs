namespace RateLimiter.Api.Options
{
    public class RateLimitOptions
    {
        public int Requests { get; set; }
        public int WindowSeconds { get; set; }
    }
}
