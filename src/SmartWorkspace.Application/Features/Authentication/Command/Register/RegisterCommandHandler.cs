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
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Features.Authentication.Command.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(ITokenService tokenService, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<Result<AuthResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var spec = new UserFilterSpecification(email: request.Request.Email);
            var userRepo = _unitOfWork.Repository<User>();
            var existingUser = await userRepo.GetEntityWithSpec(spec);
            if (existingUser != null) return Result<AuthResponse>.Failure("Email already exist");
            var user = _mapper.Map<User>(request.Request);

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Request.Password);
            await userRepo.AddAsync(user);
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
