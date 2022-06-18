using Infrastructure.Services.Identity.JWT;
using System.Threading.Tasks;
using System.Linq;
using Core.DTOs;
using Core.Entities;
using Core.ViewModels;
using Core.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Identity.JWT
{
    public class JwtMediateR
    {
        private IJWTManager _jwtManager { get; set; }
        public IUserManager _userManager { get; set; }
        public AppDbContext _dbContext { get; set; }

        public JwtMediateR(IJWTManager jwtManager, IUserManager userManager, AppDbContext dbContext)
        {
            _jwtManager = jwtManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(LoginCommand loginCmd)
        {
            User user = await _userManager.GetUserAsync(loginCmd.UserName, loginCmd.Password);
            if (user is not null)
            {
                return await _jwtManager.AuthenticateAsync(user);
            }
            return null;
        }
        public async Task<AuthenticationResponse> RefreshToken(string refToken)
        {
            if (!string.IsNullOrEmpty(refToken))
            {
                var isValidRefreshToken = _jwtManager.ValidateRefreshTokenAsync(refToken);

                var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refToken);

                if (refreshToken != null)
                {
                    var user = await _userManager.GetUserByIdAsync(refreshToken.UserId.ToString());
                    return await _jwtManager.AuthenticateAsync(user);
                }
            }
            return null;
        }
    }
}
