using Microsoft.Extensions.Options;
using Moq;
using RateLimiter.Api.Options;
using RateLimiter.Api.Services;
using RateLimiter.Api.Stores;

namespace RateLimiter.Tests.Services;

public class RateLimiterServiceTests
{
    private RateLimiterService CreateService(int requests, int windowSeconds)
    {
        var options = new RateLimitOptions
        {
            Requests = requests,
            WindowSeconds = windowSeconds
        };

        var optionsMonitorMock = new Mock<IOptionsMonitor<RateLimitOptions>>();
        optionsMonitorMock
            .Setup(o => o.CurrentValue)
            .Returns(options);

        var store = new InMemoryRateLimitStore();
        return new RateLimiterService(store, optionsMonitorMock.Object);
    }

    [Fact]
    public void Allows_Request_Within_Limit()
    {
        var service = CreateService(2, 60);

        Assert.True(service.IsAllowed("user1"));
        Assert.True(service.IsAllowed("user1"));
    }

    [Fact]
    public void Blocks_Request_When_Limit_Exceeded()
    {
        var service = CreateService(2, 60);

        Assert.True(service.IsAllowed("user1"));
        Assert.True(service.IsAllowed("user1"));
        Assert.False(service.IsAllowed("user1"));
    }

    [Fact]
    public void Allows_Requests_For_Different_Users()
    {
        var service = CreateService(1, 60);

        Assert.True(service.IsAllowed("user1"));
        Assert.True(service.IsAllowed("user2"));
    }
}
