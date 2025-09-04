using SmartWorkspace.Application.Features.Authentication.DTOs;

namespace SmartWorkspace.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
        Task<Guid> RegisterAsync(RegisterUserRequest request);
    }
}