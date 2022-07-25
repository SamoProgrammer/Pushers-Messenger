using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Entities;
using Core.ViewModels;

namespace Core.Interfaces
{
    public interface IJWTManager
    {
        public string GenerateToken(string secretKey, string issure, string audience, double ExpireTime, IEnumerable<Claim> claims = null);
        public bool ValidateRefreshToken(string refreshToken);
        public Task<AuthenticationResponse> AuthenticateAsync(User user);
        public string GenerateAccessToken(User user);
        Task<bool> SignoutAsync(string refreshToken, int userId);
    }
}
