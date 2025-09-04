
using MediatR;
using Microsoft.AspNetCore.Http;
using SmartWorkspace.Application.Features.Auth.Login;
using SmartWorkspace.Application.Features.Authentication.Command.RefreshTokens;
using SmartWorkspace.Application.Features.Authentication.Command.Register;
using SmartWorkspace.Application.Features.Authentication.Command.RevokeToken;
using SmartWorkspace.Application.Features.Authentication.DTOs;
using SmartWorkspace.Domain.Users.Models;

namespace SmartWorkspace.API.Endpoints
{
    public static class AuthEnpoints
    {
        public static void MapAuthEnpoints(this IEndpointRouteBuilder app)
        {
            var authGroup = app.MapGroup("/auth")
                .WithTags("Authentication")
                .WithOpenApi();

            authGroup.MapPost("/login", LoginAsync)
                .WithName("Login")
                .WithSummary("User Login")
                .Produces<AuthResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized);

            authGroup.MapPost("/register", RegisterAsync)
                .WithName("Register")
                .WithSummary("User Registration")
                .Produces<AuthResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            authGroup.MapPost("/refresh-token", RefreshTokenAsync)
                .WithName("RefreshToken")
                .WithSummary("Refresh access token")
                .Produces<AuthResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized);

            authGroup.MapPost("/revoke-token", RevokeTokenAsync)
                .WithName("RevokeToken")
                .WithSummary("Revoke refresh token")
                .Produces<AuthResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }

        private static async Task<IResult> LoginAsync(LoginRequest request, IMediator mediator)
        {
            try
            {
                var command = new LoginUserCommand(request);
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Data);
                }

                return Results.BadRequest(new { message = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An error occurred while processing the login request"
                );
            }
        }

        private static async Task<IResult> RegisterAsync(RegisterUserRequest request, IMediator mediator)
        {
            try
            {
                var command = new RegisterUserCommand(request);
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    return Results.Created($"/auth/user/{result.Data?.User.Id}", result.Data);
                }

                return Results.BadRequest(new { message = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An error occurred while processing the login request"
                );
            }
        }

        private static async Task<IResult> RefreshTokenAsync(RefreshTokenRequest request, IMediator mediator)
        {
            try
            {
                var command = new RefreshTokenCommand(request);
                var result = await mediator.Send(command);
                if (result.IsSuccess) return Results.Ok(result.Data);
                return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An error occurred while refreshing token"
                );
            }
        }

        private static async Task<IResult> RevokeTokenAsync(RevokeTokenRequest request, IMediator mediator)
        {
            try
            {
                var command = new RevokeTokenCommand(request);
                var result = await mediator.Send(command);
                if (result.IsSuccess) return Results.Ok(new {message = "Token revoke successfully"});
                return Results.BadRequest(new { message = result.ErrorMessage});
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An error occurred while revoke the token"
                );
            }
        }
    }
}
