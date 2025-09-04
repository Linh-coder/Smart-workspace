using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Domain.Entities;
using SmartWorkspace.Domain.Entities.Users;
using SmartWorkspace.Domain.Repositories;
using SmartWorkspace.Domain.Specifications;
using SmartWorkspace.Persistence.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;
        private readonly int _maxActiveTokenPerUser;

        public TokenService(IConfiguration configuration, IUnitOfWork uow)
        {
            _configuration = configuration;
            _uow = uow;
            _jwtSecret = configuration["jwt:Secret"] ?? throw new InvalidOperationException("JWT secret not configuration");
            _issuer = configuration["jwt:Issuer"] ?? "SmartWorkspace";
            _audience = configuration["jwt:Audience"] ?? "SmartWorkspace";
            _accessTokenExpirationMinutes = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60");
            _refreshTokenExpirationDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            _maxActiveTokenPerUser = int.Parse(configuration["Jwt:MaxActiveTokenPerUser"] ?? "5");
        }

        public async Task<TokenResult> GenerateTokenAsync(User user)
        {
            await CleanupExpiredTokenAsync(user.Id);
            await EnforceTokenLimitAsync(user.Id);

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            var expiredAt = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

            // Store refreshToken in DB
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpriesAt = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays),
                UserId = user.Id,
                IsRevoked = false
            };


            
            await _uow.Repository<RefreshToken>().AddAsync(refreshTokenEntity);
            await _uow.SaveChangeAsync(); 

            return new TokenResult(accessToken, refreshToken, expiredAt);
        }

        public async Task<TokenResult> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenSpec = new RefreshTokenFilterSpecification(token: refreshToken);
            var refreshTokenRepo = _uow.Repository<RefreshToken>();
            var storedToken = await refreshTokenRepo.GetEntityWithSpec(refreshTokenSpec);

            if (storedToken != null) throw new SecurityTokenException("Invalid refresh Token");

            if (storedToken.IsActive)
            {
                // Token reuse detection - revoke all descendant tokens
                await RevokeDescendantRefreshTokens(storedToken, "Token reuse detected");
                throw new SecurityTokenException("Invalid refresh Token");
            }

            //Get user
            var userRepo = _uow.Repository<User>();
            var user = await userRepo.GetByIdAsync(storedToken.UserId);
            if (user == null) throw new SecurityTokenException("User not found");

            // Rotate refresh token
            storedToken.IsRevoked = true;
            storedToken.RevokeAt = DateTime.UtcNow;
            storedToken.RevokeReason = "Replace by new token";
            refreshTokenRepo.Update(storedToken);

            // Generate new Token
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshTokenString = GenerateRefreshToken();
            var newExpiredAt = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

            // Store new refresh token
            var newRefreshToken = new RefreshToken
            {
                Token = newRefreshTokenString,
                ExpriesAt = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays),
                UserId = user.Id,
                IsRevoked = false,
            };

            // Link Token for tracking
            storedToken.ReplaceByToken = newRefreshTokenString;

            await refreshTokenRepo.AddAsync(newRefreshToken);
            await _uow.SaveChangeAsync();

            return new TokenResult(newAccessToken, newRefreshTokenString, newExpiredAt);
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken, string? reason = null, string? revokeByIp = null)
        {
            var spec = new RefreshTokenFilterSpecification(token: refreshToken);
            var refreshTokenRepo = _uow.Repository<RefreshToken>();
            var storedToken = await refreshTokenRepo.GetEntityWithSpec(spec);

            if (storedToken != null || storedToken.IsRevoked) return false;

            // Revoke Token and all descendants
            storedToken.IsRevoked |= true;
            storedToken.RevokeAt = DateTime.UtcNow;
            storedToken.RevokeReason = reason ?? "Manual revocation";
            storedToken.RevokeByIp = revokeByIp;

            refreshTokenRepo.Update(storedToken);
            await RevokeDescendantRefreshTokens(storedToken, reason);
            await _uow.SaveChangeAsync();
            return true;
        }

        #region Private Method
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
        private async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string reason)
        {
            if (!string.IsNullOrEmpty(refreshToken.ReplaceByToken))
            {
                var spec = new RefreshTokenFilterSpecification(token: refreshToken.ReplaceByToken);
                var refreshTokenRepo = _uow.Repository<RefreshToken>();
                var childToken = await refreshTokenRepo.GetEntityWithSpec(spec);

                if (childToken?.IsActive == true)
                {
                    childToken.IsRevoked = true;
                    childToken.RevokeAt = DateTime.UtcNow;
                    childToken.RevokeReason = reason ?? "Revoke due to ancestor token reuse";
                    refreshTokenRepo.Update(childToken);
                    await RevokeDescendantRefreshTokens(childToken, reason);
                }
            }
        }

        private async Task CleanupExpiredTokenAsync(Guid userId)
        {
            var spec = new RefreshTokenFilterSpecification(userId: userId, onlyExpired: true);
            var refreshTokenRepo = _uow.Repository<RefreshToken>();
            var expriedTokens = await refreshTokenRepo.GetListAsync(spec);

            foreach (var token in expriedTokens)
            {
                refreshTokenRepo.Delete(token);
            }

            if (expriedTokens.Any()) await _uow.SaveChangeAsync();
        }

        private async Task EnforceTokenLimitAsync(Guid userId)
        {
            var spec = new RefreshTokenFilterSpecification(userId: userId, onlyActive: true);
            var refreTokenRepo = _uow.Repository<RefreshToken>();
            var activeTokens = (await refreTokenRepo.GetListAsync(spec)).ToList();
            if (activeTokens.Count >= _maxActiveTokenPerUser)
            {
                var tokenToRevoke = activeTokens.Take(activeTokens.Count - _maxActiveTokenPerUser + 1);
                foreach (var token in tokenToRevoke)
                {
                    token.IsRevoked = true;
                    token.RevokeAt = DateTime.UtcNow;
                    token.RevokeReason = "Token limit exceed";
                    refreTokenRepo.Update(token);
                }
                await _uow.SaveChangeAsync();
            }

        }
        #endregion
    }
}
