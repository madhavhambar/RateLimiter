namespace RateLimiter.Api.Interfaces
{
    public interface IRateLimiterService
    {
        bool IsAllowed(string identifier);
    }
}
