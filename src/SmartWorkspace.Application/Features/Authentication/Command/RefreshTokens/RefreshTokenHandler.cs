using MediatR;
using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Application.Features.Authentication.DTOs;
using SmartWorkspace.Domain.Entities;
using SmartWorkspace.Domain.Repositories;
using SmartWorkspace.Domain.Specifications;
using SmartWorkspace.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Features.Authentication.Command.RefreshTokens
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _uow;

        public RefreshTokenHandler(ITokenService tokenService, IUnitOfWork uow)
        {
            _tokenService = tokenService;
            _uow = uow;
        }

        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenResult = await _tokenService.RefreshTokenAsync(request.Request.RefreshToken);
            var spec = new RefreshTokenFilterSpecification(token: request.Request.RefreshToken, onlyActive: true);
            var user = (await _uow.Repository<RefreshToken>().GetEntityWithSpec(spec)).User;

            if (user == null || user.IsActive) return Result<AuthResponse>.Failure("Invalid Credentials");

            var userDTO = new UserDTO(user.Id, user.FullName, user.Email, user.CreatedAt);
            var authResponse = new AuthResponse(
                tokenResult.AccessToken,
                tokenResult.RefreshToken,
                tokenResult.ExpiresAt,
                userDTO
            );

            return Result<AuthResponse>.Success(authResponse);
        }
    }
}
