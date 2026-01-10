using RateLimiter.Api.Interfaces;
using RateLimiter.Api.Options;
using RateLimiter.Api.Services;
using RateLimiter.Api.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RateLimitOptions>(
    builder.Configuration.GetSection("RateLimiting"));

builder.Services.AddSingleton<IRateLimitStore, InMemoryRateLimitStore>();
builder.Services.AddScoped<IRateLimiterService, RateLimiterService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
