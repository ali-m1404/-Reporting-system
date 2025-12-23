using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime? RegisterDate { get; set; }

        // فقط برای منطق داخلی
        public int? RoleId { get; set; }

        // فقط برای نمایش
        public string RoleTitle { get; set; }

        public UserDto(User user)
        {
            Id = user.Id;
            UserName = user.FirstName;
            ProfileImagePath = user.ProfileImage;
            Email = user.Email;
            Phone = user.PhoneNumber;
            IsActive = user.IsActive;
            RegisterDate = user.CreatedDate;

            RoleId = user.RoleId;

            RoleTitle = user.RoleId switch
            {
                3 => "کاربر عادی",
                2 => "ادمین",
                1 => "سوپر ادمین",
                _ => "نامشخص"
            };
        }
    }
}
