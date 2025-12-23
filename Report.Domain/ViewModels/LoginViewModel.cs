using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="لطفا ایمیل یا نام کاربری را وارید کنید ")]
        [MaxLength(250)]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage ="کلمه عبور را وارد کنید")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}
