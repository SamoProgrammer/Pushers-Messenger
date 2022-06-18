using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Identity.JWT
{
    public class JWTSettings
    {
        public string AccessTokenSecret { get; set; }
        public string RefreshTokenSecret { get; set; }
        public double AccessTokenExpirationMinutes { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
