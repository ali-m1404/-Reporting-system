using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Report.Domain.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام کاربری الزامی است")]
        [StringLength(50, ErrorMessage = "نام کاربری نمی‌تواند بیشتر از ۵۰ کاراکتر باشد")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
        public string Email { get; set; }

        [Required(ErrorMessage = "انتخاب نقش الزامی است")]
        public int? RoleId { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }
    }
}

