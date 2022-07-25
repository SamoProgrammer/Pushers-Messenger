using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp.DTOs.Account
{
    internal class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public string Error { get; set; }
    }
}
