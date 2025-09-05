using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace SmartWorkspace.API.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdKey = "CorrelationId";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = GetOrGenerateCorrelationId(context);
            using(LogContext.PushProperty(CorrelationIdKey, correlationId))
            {
                context.Response.Headers.Add(CorrelationIdKey, correlationId);
                await _next(context);
            }
        }

        private static string GetOrGenerateCorrelationId(HttpContext context) 
            => context.Request.Headers[CorrelationIdKey].FirstOrDefault() ?? Guid.NewGuid().ToString();
    }
}
