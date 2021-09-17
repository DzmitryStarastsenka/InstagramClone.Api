using InstagramClone.Api.CustonMiddleware;
using Microsoft.AspNetCore.Builder;

namespace InstagramClone.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}