using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Persistence.Context;
using Infrastructure.Services.Identity.Customization;
using Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Services.Identity.JWT;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Infrastructure.Services
{
    public static class Configurations
    {
        public static void Add(IServiceCollection services,IConfiguration configuration)
        {
            var jwtSettings = new JWTSettings();
            configuration.Bind(nameof(JWTSettings), jwtSettings);
            services.AddSingleton(jwtSettings);


            services.AddScoped<JwtMediateR>();

            services.AddScoped<IUserManager, Infrastructure.Services.Identity.UserManager>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Server")));


            services.AddIdentity<User,IdentityRole<int>>().AddEntityFrameworkStores<AppDbContext>().AddErrorDescriber<PersianIdentityErrorDescriber>().AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);
            services.AddAuthentication(options=> {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt=> {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(jwtSettings.AccessTokenSecret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = false,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                options.User.RequireUniqueEmail = true;
            });

            services.AddScoped<IJWTManager, JWTManager>();
        }
    }
}
