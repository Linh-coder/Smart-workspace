using SmartWorkspace.Application.Common.Attributes;
using SmartWorkspace.Application.Common.Interfaces;

namespace SmartWorkspace.API.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IPermissionService permissionService, CancellationToken ct)
        {
            var user = context.User;
            if(user.Identity?.IsAuthenticated ?? false)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var endpoint = context.GetEndpoint();
            var requiredPermission = endpoint?.Metadata.GetMetadata<RequirePermissionAttribute>()?.Permission;

            if (requiredPermission != null) 
            {
                var usedId = user.FindFirst("sub")?.Value;
                var workspaceId = context.Request.Headers["X-Workspace-Id"].FirstOrDefault();

                if (usedId == null || workspaceId == null) 
                { 
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("WorkspaceId required");
                    return;
                }

                var hasAccess = await permissionService.HasPermissionAsync(Guid.Parse(usedId), Guid.Parse(workspaceId), requiredPermission, ct);

                if (!hasAccess) 
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }
            }

            await _next(context);
        }
    }
}
