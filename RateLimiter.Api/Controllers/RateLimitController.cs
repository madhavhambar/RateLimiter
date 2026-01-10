using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RateLimiter.Api.Interfaces;
using RateLimiter.Api.Models;
using System.Threading.RateLimiting;

namespace RateLimiter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateLimitController : ControllerBase
    {
        private readonly IRateLimiterService _rateLimiter;
        public RateLimitController(IRateLimiterService rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        [HttpPost("check")]
        public IActionResult Check([FromBody] RateLimitRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Identifier))
                return BadRequest("Identifier is required.");

            var allowed = _rateLimiter.IsAllowed(request.Identifier);

            return allowed
                ? Ok()
                : StatusCode(StatusCodes.Status429TooManyRequests);
        }
    }
}
