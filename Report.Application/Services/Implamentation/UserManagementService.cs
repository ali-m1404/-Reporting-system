using Domain.Entities.Users;
using Report.Application.Dtos;
using Report.Domain.Contracts;
using Report.Domain.Entities;
using Report.Domain.ViewModels;
using Report.Services.Contracts;


namespace Report.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository2 _repository;


        public UserManagementService(IUserRepository2 repository)
        {
            _repository = repository;
        }


        public async Task<List<UserDto>> GetUsersAsync(string search, string role, string status)
        {
            var users = await _repository.GetUsersAsync(search, role, status);

            return users.Select(u => new UserDto(u)).ToList();
        }

        #region Updateusrinfo
        public async Task<EditUserViewModel> GetUserForEditAsync(int userId)
        {
            var user = await _repository.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            return new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.FirstName,
                Email = user.Email,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
              
            };
        }

        public async Task UpdateUserAsync(EditUserViewModel model, int currentUserRoleId)
        {
            var user = await _repository.GetByIdAsync(model.Id)
                       ?? throw new Exception("کاربر پیدا نشد");

            // ❌ اگر کاربر لاگین شده SuperAdmin نیست
            // و می‌خواهد رول SuperAdmin را تغییر دهد
            if (currentUserRoleId != 1 && user.RoleId == 1 && model.RoleId != 1)
            {
                throw new Exception("شما اجازه تغییر نقش سوپرادمین را ندارید");
            }

            // ✅ فقط SuperAdmin می‌تواند رول‌ها را تغییر دهد
            if (currentUserRoleId != 1 && model.RoleId != user.RoleId)
            {
                throw new Exception("شما اجازه تغییر نقش کاربران را ندارید");
            }
            if(currentUserRoleId != 1 && model.RoleId == 1)
            {
                throw new Exception("شما اجازه تغییر وضعییت این کاربر را ندارید");
            }

            // تغییر وضعیت و نقش
            user.IsActive = model.IsActive;
            user.RoleId = model.RoleId;

            // تغییر نام کاربری
            if (!string.IsNullOrWhiteSpace(model.UserName) && model.UserName != user.FirstName)
            {
                var exists = await _repository.ExistsUserNameAsync(model.UserName);
                if (exists)
                    throw new Exception("این نام کاربری قبلاً ثبت شده است");

                user.FirstName = model.UserName;
            }

            // تغییر ایمیل
            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                var exists = await _repository.ExistsEmailAsync(model.Email);
                if (exists)
                    throw new Exception("این ایمیل قبلاً ثبت شده است");

                user.Email = model.Email;
            }

            await _repository.SaveAsync();
        }


        #endregion

        public async Task DisableUserAsync(int userId, string performedBy)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");


            if (user.RoleId == 1) throw new Exception("Cannot disable SuperAdmin");


            if (await _repository.IsLastAdminAsync(userId))
                throw new Exception("Cannot disable last Admin");


            user.IsActive = false;
            await _repository.UpdateAsync(user);
        }


        public async Task EnableUserAsync(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");


            user.IsActive = true;
            await _repository.UpdateAsync(user);
        }


        public async Task ChangeUserRoleAsync(int userId, int newRole, string performedBy)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");


            if (user.RoleId == 1) throw new Exception("Cannot change SuperAdmin role");


            user.RoleId = newRole;
            await _repository.UpdateAsync(user);
        }


        public async Task ResetPasswordAsync(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");


            user.PasswordHash = Guid.NewGuid().ToString(); // placeholder
            await _repository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            // 🔒 اگر خواستی محدودیت بگذاری (مثلاً حذف SuperAdmin)
            if (user.RoleId == 1)
                throw new Exception("Cannot delete SuperAdmin");

            await _repository.DeleteAsync(user);
            await _repository.SaveAsync();
        }



        
    }
}