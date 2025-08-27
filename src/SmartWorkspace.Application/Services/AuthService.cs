using Microsoft.AspNetCore.Identity;
using SmartWorkspace.Application.DTOs;
using SmartWorkspace.Application.Interfaces;
using SmartWorkspace.Domain.Repositories;
using SmartWorkspace.Domain.Specifications;
using SmartWorkspace.Domain.Specifications.UserSpecifications;
using SmartWorkspace.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Services
{
    public class AuthService
    {
        //private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            //_userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<LoginResponse> LoginAsync (LoginRequest loginRequest)
        {
            //var user = await _userRepository.GetByUserNameAsync(loginRequest.UserName);
            var spec = new UserFilterSpecification(name: loginRequest.UserName);
            var user = await _unitOfWork.Repository<User>().GetEntityWithSpec(spec);
            if (user == null) throw new Exception("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);
            if (result == PasswordVerificationResult.Failed) throw new Exception("Invalid credentials");

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new LoginResponse(token);
        }

        public async Task<Guid> RegisterAsync(RegisterUserRequest request)
        {
            var user = new User()
            {
                FullName = request.UserName,
                Email = request.Email,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _unitOfWork.Repository<User>().AddAsync(user);
            return user.Id;
        }
    }
}
