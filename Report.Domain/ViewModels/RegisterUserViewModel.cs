using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Report.Domain.ViewModels
{
    public class RegisterUserViewModel
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "فرمت {0} معتبر نیست")]
        [MaxLength(150, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }

        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Phone(ErrorMessage = "فرمت {0} معتبر نیست")]
        [MaxLength(20, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(6, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$",
                           ErrorMessage = "کلمه عبور باید حداقل ۸ کاراکتر، شامل حروف بزرگ و کوچک، عدد و نماد باشد")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Compare("Password", ErrorMessage = "کلمه‌های عبور یکسان نیستند")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

        // ✅ اصلاح: فقط یک فیلد برای هر فایل
        [Display(Name = "تصویر پروفایل")]
        [DataType(DataType.Upload)]
        public IFormFile? ProfileImage { get; set; } // فقط این یکی کافی است

        [Display(Name = "عکس تذکره")]
        [DataType(DataType.Upload)]
        // [Required(ErrorMessage = "لطفا {0} را آپلود کنید")]
        public IFormFile? IdentityImage { get; set; } // ✅ تغییر از string به IFormFile
    }
}