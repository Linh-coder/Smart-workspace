using Serilog;
using Serilog.Events;
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

        public static WebApplicationBuilder AddSerilLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithProperty("Application", "SmartworkSpace")
                .Enrich.WithProperty("Enviroment", builder.Environment.EnvironmentName)
                .Enrich.WithProperty("Version", typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0")
                .CreateLogger();

            builder.Host.UseSerilog();
            return builder;
        }
        public static void MapAllEndpoints(this WebApplication app)
        {
            app.MapAuthEnpoints();
            // Add other custom middlewares here
        }

        #region Custom Middleware

        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static WebApplication UseSerilogMiddleware(this WebApplication app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseSerilogRequestLogging(options =>
            {
                // Add serilog request logging middleware
                options.MessageTemplate = "HTTP {RequestMethod} responded {StatusCode} in {Elapsed:0.000} ms";
                options.GetLevel = GetLogLevel;
                options.EnrichDiagnosticContext = EnrichFromRequest;
            });

            return app;
        }

        private static LogEventLevel GetLogLevel(HttpContext httpContext, double elapsedMs, Exception? ex)
        {
            if (ex != null) return LogEventLevel.Error;
            
            var statusCode = httpContext.Response.StatusCode;
            if (statusCode >= 500) return LogEventLevel.Error;
            if (statusCode >= 400) return LogEventLevel.Warning;
            if (elapsedMs > 4000) return LogEventLevel.Warning;

            return LogEventLevel.Information;
        }

        private static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.FirstOrDefault());
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());

            if (httpContext.User?.Identity?.IsAuthenticated == true)
                diagnosticContext.Set("UserId", httpContext.User.FindFirst("sub")?.Value);
        }

        #endregion

    }
}
