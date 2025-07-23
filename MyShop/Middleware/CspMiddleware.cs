using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyShop.Middleware
{
    public class CspMiddleware
    {
        private readonly RequestDelegate _next;
        public CspMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers["Content-Security-Policy"] =
                "default-src 'self'; script-src 'self'; style-src 'self'; object-src 'none'";
            await _next(context);
        }
    }
}
