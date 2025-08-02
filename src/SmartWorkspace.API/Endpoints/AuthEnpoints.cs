namespace SmartWorkspace.API.Endpoints
{
    public static class AuthEnpoints
    {
        public static void MapAuthEnpoints(this IEndpointRouteBuilder app)
        {
            // Define authentication endpoints here
            app.MapPost("/auth/login", async () =>
            {
                return Results.Ok("Login endpoint");
            });

            app.MapPost("/auth/register", async () =>
            {
                return Results.Ok("Register endpoint");
            });
        }
    }
}
