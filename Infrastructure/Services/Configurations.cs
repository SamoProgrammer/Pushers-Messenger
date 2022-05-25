using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Context;
using Infrastructure.Services.Identity.Customization;
using Core.Entities;

namespace Infrastructure.Services
{
    public static class Configurations
    {
        public static void Add(IServiceCollection services,string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));


            services.AddIdentity<User,IdentityRole<int>>().AddEntityFrameworkStores<AppDbContext>().AddErrorDescriber<PersianIdentityErrorDescriber>().AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);


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
        }
    }
}
