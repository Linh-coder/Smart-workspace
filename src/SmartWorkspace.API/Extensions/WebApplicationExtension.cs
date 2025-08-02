using SmartWorkspace.API.Endpoints;
using SmartWorkspace.API.Middlewares;

namespace SmartWorkspace.API.Extensions
{
    public static class WebApplicationExtension
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void MapAllEndpoints(this WebApplication app)
        {
            app.MapAuthEnpoints();
            // Add other custom middlewares here
        }
    }
}
