using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class AppDbContext:IdentityDbContext<User,IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Friend> Friends { get; set; }
    }
}
