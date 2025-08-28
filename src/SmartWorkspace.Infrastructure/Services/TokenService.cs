using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartWorkspace.Application.Interfaces;
using SmartWorkspace.Domain.Entities.Users;
using SmartWorkspace.Persistence.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;

        public TokenService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _jwtSecret = configuration["jwt:Secret"] ?? throw new InvalidOperationException("JWT secret not configuration");
            _issuer = configuration["jwt:Issuer"] ?? "SmartWorkspace";
            _audience = configuration["jwt:Audience"] ?? "SmartWorkspace";
            _accessTokenExpirationMinutes = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60");
            _refreshTokenExpirationDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays" ?? "7"]);
        }

        public async Task<TokenResult> GenerateTokenAsync(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            var expiredAt = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

            return new TokenResult(accessToken, refreshToken, expiredAt);
        }

        public Task<TokenResult> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        private string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("sub", user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FullName),
                new("jti", Guid.NewGuid().ToString()),
                new("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
