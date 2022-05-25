using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Identity
{
    public class UserViewModel
    {
        #region Email
        [MaxLength(250, ErrorMessage = "حداکثر تعداد کاراکتر 250 است")]
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نیست ")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Email { get; set; }
        #endregion
        #region UserName
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(250, ErrorMessage = "حداکثر تعداد کاراکتر 250 است")]
        [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "نام کاربری باید فقط شامل عدد و حروف انگلیسی باشد ")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        #endregion
        #region Password
        [MaxLength(250, ErrorMessage = "حداکثر تعداد کاراکتر 250 است")]
        [Display(Name = "کلمه ی عبور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [RegularExpression(pattern: "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#%^&])[A-Za-z0-9@#%^&]{6,}", ErrorMessage = "رمز عبور باید حداقل دارای 6 کاراکتر و حداقل یک حروف کوچک و بزرگ انگلیسی و یک کاراکتر خاص (@#%^&) باشد")]
        public string Password { get; set; }
        #endregion
        #region ConfirmPassword
        [MaxLength(250, ErrorMessage = "حداکثر تعداد کاراکتر 250 است")]
        [Display(Name = "تکرار کلمه ی عبور ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Compare(nameof(Password), ErrorMessage = "تکرار کلمه ی عبور با کلمه ی عبور مطابقت ندارد")]
        public string ConfirmPassword { get; set; }
        #endregion
    }
}
