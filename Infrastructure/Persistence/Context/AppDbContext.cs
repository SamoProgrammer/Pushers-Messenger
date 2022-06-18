using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public class AppDbContext:IdentityDbContext<User,IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
