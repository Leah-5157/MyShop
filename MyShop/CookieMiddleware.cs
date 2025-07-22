using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyShop
{
    public class CookieMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["jwt_token"];
            if (!string.IsNullOrEmpty(token))
            {
                // Inject token into Authorization header for JWT Bearer middleware
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }
            await _next(context);
        }
    }
}
