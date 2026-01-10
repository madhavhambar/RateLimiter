namespace RateLimiter.Api.Interfaces
{
    public interface IRateLimitStore
    {
        int Increment(string identifier, DateTime windowStart);
    }
}
