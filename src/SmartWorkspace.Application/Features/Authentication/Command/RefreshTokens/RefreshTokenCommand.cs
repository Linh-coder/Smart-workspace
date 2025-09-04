using MediatR;
using SmartWorkspace.Application.Features.Authentication.DTOs;
using SmartWorkspace.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Features.Authentication.Command.RefreshTokens
{
    public record RefreshTokenCommand(RefreshTokenRequest Request) : IRequest<Result<AuthResponse>>;
}
