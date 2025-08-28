using SmartWorkspace.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResult> GenerateTokenAsync(User user);
        Task<TokenResult> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }

    public record TokenResult(
      string AccessToken,
      string RefreshToken,
      DateTime ExpiresAt
    );
}
