using System.Collections.Generic;

namespace DesktopApp.DTOs.Account
{
    internal class RegisterResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
