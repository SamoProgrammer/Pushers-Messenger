using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp.DTOs.Account
{
    internal class RegisterResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
