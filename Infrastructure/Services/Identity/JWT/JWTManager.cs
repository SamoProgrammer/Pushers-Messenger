using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Core.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Services.Identity.JWT
{
    public class JWTManager : IJWTManager
    {
        private readonly UserManager<User> _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly AppDbContext _appDbContext;

        public JWTManager(UserManager<User> userManager, JWTSettings jwtSettings,AppDbContext appDbContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _appDbContext = appDbContext;
        }
        public string GenerateToken(string secretKey, string issure, string audience, double ExpireTime, IEnumerable<Claim> claims = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(issuer: issure,
                    audience: audience,
                    claims: claims, DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(ExpireTime),
                    signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        public bool ValidateRefreshTokenAsync(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecret)),
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            try
            {
                jwtSecurityTokenHandler.ValidateToken(refreshToken, validationParameters,
                    out SecurityToken _);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<AuthenticationResponse> AuthenticateAsync(User user)
        {
            var refreshToken = GenerateToken(_jwtSettings.RefreshTokenSecret,_jwtSettings.Issuer,_jwtSettings.Audience,_jwtSettings.RefreshTokenExpirationMinutes);
            var userRefreshTokens = _appDbContext.RefreshTokens.Where(r => r.UserId == user.Id);
            if (userRefreshTokens.Count()!=0)
            {
                _appDbContext.RefreshTokens.RemoveRange(userRefreshTokens);
                _appDbContext.SaveChanges();
            }
            await _appDbContext.RefreshTokens.AddAsync(new RefreshToken() {UserId=user.Id,Token=refreshToken });
            await _appDbContext.SaveChangesAsync();
            return new AuthenticationResponse
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = refreshToken
            };
        }
        public string GenerateAccessToken(User user)
        {
            var claims = new Claim[]
                {
                new Claim(ClaimTypes.Name,user.UserName ),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                };
            return GenerateToken(_jwtSettings.AccessTokenSecret,_jwtSettings.Issuer, _jwtSettings.Audience, _jwtSettings.AccessTokenExpirationMinutes, claims);
        }
    }
}
