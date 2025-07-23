using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MyShop.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> _requests = new();
        private const int LIMIT = 10;
        private static readonly TimeSpan WINDOW = TimeSpan.FromMinutes(1);

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Apply rate limiting only to the login endpoint
            if (context.Request.Path.StartsWithSegments("/api/Users/Login", StringComparison.OrdinalIgnoreCase))
            {
                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var now = DateTime.UtcNow;
                var entry = _requests.GetOrAdd(ip, _ => (0, now));

                if (now - entry.WindowStart > WINDOW)
                {
                    entry = (0, now);
                }

                if (entry.Count >= LIMIT)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                    return;
                }

                _requests[ip] = (entry.Count + 1, entry.WindowStart);
            }
            await _next(context);
        }
    }
}
