using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Report.Domain.ViewModels
{
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل معتبر نیست")]
        public string Email { get; set; }

        [Required(ErrorMessage = "شماره تماس الزامی است")]
        [Phone(ErrorMessage = "شماره تماس معتبر نیست")]
        public string PhoneNumber { get; set; }

        public IFormFile? ProfileImage { get; set; }

        // 🔐 تغییر رمز عبور
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "رمز عبور جدید حداقل ۶ کاراکتر باشد")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "تکرار رمز عبور مطابقت ندارد")]
        public string? ConfirmNewPassword { get; set; }
    }
}
