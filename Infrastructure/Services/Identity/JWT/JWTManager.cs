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

namespace Infrastructure.Services.Identity.JWT
{
    public class JWTManager : IJWTManager
    {
		private readonly IConfiguration iconfiguration;
		private readonly UserManager<User> _userManager;
		public JWTManager(IConfiguration iconfiguration,UserManager<User> userManager)
		{
			this.iconfiguration = iconfiguration;
			_userManager = userManager;
		}
		public async Task<JwtToken> Authenticate(LoginCommand logincmd)
        {
			User user = await _userManager.FindByNameAsync(logincmd.UserName);
			if (user != null && await _userManager.CheckPasswordAsync(user,logincmd.Password))
			{
				return null;
			}

			// Else we generate JSON Web Token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
			  {
			 new Claim(ClaimTypes.Name,user.UserName ),
			 new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
			  }),
				Expires = DateTime.UtcNow.AddMinutes(10),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return new JwtToken { Token = tokenHandler.WriteToken(token) };
		}
    }
}
