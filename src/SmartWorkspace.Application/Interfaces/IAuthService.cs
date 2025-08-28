using SmartWorkspace.Application.DTOs;

namespace SmartWorkspace.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<Guid> RegisterAsync(RegisterUserRequest request);
    }
}