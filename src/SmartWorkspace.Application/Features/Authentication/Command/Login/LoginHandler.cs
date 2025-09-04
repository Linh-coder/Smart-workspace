using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Application.Features.Authentication.DTOs;
using SmartWorkspace.Domain.Entities.Users;
using SmartWorkspace.Domain.Repositories;
using SmartWorkspace.Domain.Specifications.UserSpecifications;
using SmartWorkspace.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Features.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public LoginHandler(ITokenService tokenService, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var spec = new UserFilterSpecification(email: request.Request.Email);
            var userRepo = _unitOfWork.Repository<User>();
            var user = await userRepo.GetEntityWithSpec(spec);

            if (user == null || user.IsActive) return Result<AuthResponse>.Failure("Invalid Credentials");

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Request.Password);
            if(verificationResult == PasswordVerificationResult.Failed) return Result<AuthResponse>.Failure("Invalid Credentials");

            // Update Last Login
            user.LastLoginAt = DateTime.UtcNow;
            userRepo.Update(user);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var tokenResult = await _tokenService.GenerateTokenAsync(user);

            var userDTO = _mapper.Map<UserDTO>(request.Request);
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
