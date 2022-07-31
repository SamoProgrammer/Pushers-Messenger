using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class RegisterationCommand
    {
        public string Email { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(50)]
        public string ConfirmPassword { get; set; }
        [MaxLength(30)]
        public string UserName { get; set; }
    }
}
