using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWorkspace.Domain.Entities.Users;

namespace SmartWorkspace.Application.Features.Authentication.DTOs
{
    public record AuthResponse(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        UserDTO User
    );

    public record UserDTO(
        Guid Id,
        string FullName,
        string Email,
        DateTime CreatedAt
    );
}
