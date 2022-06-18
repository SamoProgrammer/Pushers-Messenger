using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Core.Entities;
using Core.Interfaces;
using Core.ViewModels;

namespace Infrastructure.Services.Identity
{
    public class UserManager : IUserManager
    {
        private UserManager<User> _userManager;

        public UserManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> ConfirmEmail(string username, string token)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace("%2F", "/");
                var res = await _userManager.ConfirmEmailAsync(_userManager.FindByNameAsync(username).Result, token);
                return res.Succeeded;
            }

            return false;
        }

        public async Task<RegisterationResponse> Register(RegisterationCommand command)
        {
            RegisterationResponse response = new RegisterationResponse();
            User user = new User()
            {
                Email = command.Email,
                UserName = command.UserName,
            };

            if (command.Password != command.ConfirmPassword)
            {
                response.Success = false;
                response.Errors = new List<string>();
                response.Errors.Add("کلمه ی عبور با تکرار کلمه ی عبور مطابقت ندارد");
                return response;
            }
            //Add user to database
            IdentityResult res = await _userManager.CreateAsync(user, command.Password);
            if (res.Succeeded)
            {
                response.Errors = null;
                response.Success = true;
                response.EmailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return response;
            }

            response.Success = false;
            response.Errors = new List<string>();
            foreach (var error in res.Errors)
            {
                response.Errors.Add(error.Description);
            }
            return response;
        }

        public async Task<User> GetUserAsync(string userName,string password)
        {

            User user = await _userManager.FindByNameAsync(userName);
            if (user != null && await _userManager.CheckPasswordAsync(user,password))
            {
                return user;
            }
            return null;
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}
