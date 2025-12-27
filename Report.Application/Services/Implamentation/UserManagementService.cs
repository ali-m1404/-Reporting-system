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

        public async Task UpdateUserAsync(EditUserViewModel model)
        {
            var user = await _repository.GetByIdAsync(model.Id)
                       ?? throw new Exception("User not found");

            // ❗ SuperAdmin قابل ویرایش خطرناک نیست
            if (user.RoleId == 1 && model.RoleId != 1)
                throw new Exception("Cannot change SuperAdmin role");

          

            // تغییرات وضعیت و نقش
            user.IsActive = model.IsActive;
            user.RoleId = model.RoleId;

            // ✅ اضافه کردن تغییر نام و ایمیل
            if (!string.IsNullOrWhiteSpace(model.UserName) && model.UserName != user.FirstName)
            {
                // می‌توانید چک کنید که نام تکراری نباشد
                var exists = await _repository.ExistsUserNameAsync(model.UserName);
                if (exists)
                    throw new Exception("UserName already exists");
                user.FirstName = model.UserName;
            }

            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                // می‌توانید چک کنید که ایمیل تکراری نباشد
                var exists = await _repository.ExistsEmailAsync(model.Email);
                if (exists)
                    throw new Exception("Email already exists");
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