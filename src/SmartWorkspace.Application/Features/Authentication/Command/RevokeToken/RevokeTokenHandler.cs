using MediatR;
using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Domain.Repositories;
using SmartWorkspace.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Features.Authentication.Command.RevokeToken
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, Result<bool>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _uow;

        public RevokeTokenHandler(ITokenService tokenService, IUnitOfWork uow)
        {
            _tokenService = tokenService;
            _uow = uow;
        }

        public async Task<Result<bool>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var success = await _tokenService.RevokeTokenAsync(request.Request.RefreshToken, request.Request.Reason, request.IpAddress);
            if (success) return Result<bool>.Success(true);
            return Result<bool>.Failure("Token not found or already revoked");
        }
    }
}
