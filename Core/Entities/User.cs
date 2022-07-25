using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : Microsoft.AspNetCore.Identity.IdentityUser<int>
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}


