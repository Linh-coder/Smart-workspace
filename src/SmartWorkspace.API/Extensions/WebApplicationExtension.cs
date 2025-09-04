using SmartWorkspace.API.Endpoints;
using SmartWorkspace.API.Middlewares;
using SmartWorkspace.Persistence.Seed.Common;

namespace SmartWorkspace.API.Extensions
{
    public static class WebApplicationExtension
    {
        public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();
            try
            {
                // Always apply migrations
                await databaseInitializer.InitializeAsync();

                // Seed only in development
                if (app.Environment.IsDevelopment())
                {
                    await databaseInitializer.SeedAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database initialization failed.");
                if (app.Environment.IsDevelopment()) throw;
            }

            return app;
        }

        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<PermissionMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void MapAllEndpoints(this WebApplication app)
        {
            app.MapAuthEnpoints();
            // Add other custom middlewares here
        }
    }
}
