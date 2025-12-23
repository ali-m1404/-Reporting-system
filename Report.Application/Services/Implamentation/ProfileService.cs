using MyEshop.Core.Security;
using Report.Application.Services.Interfaces;
using Report.Domain.Contracts;
using Report.Domain.ViewModels;
using MyEshop.Core.Security;


namespace Report.Application.Services.Implamentation
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository2 _userRepository;
        

        public ProfileService(IUserRepository2 userRepository2)
        {
            _userRepository = userRepository2;
            
        }

        public async Task<EditProfileViewModel> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            return new EditProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,

                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task UpdateProfileAsync(EditProfileViewModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id)
                       ?? throw new Exception("User not found");
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            // تغییر رمز عبور (اختیاری)
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if (!PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.CurrentPassword))
                    throw new Exception("رمز فعلی اشتباه است");

                user.PasswordHash = PasswordHasher.HashPassword(model.NewPassword);
            }


            // عکس پروفایل
            if (model.ProfileImage != null)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ProfileImage.FileName)}";
                string path = Path.Combine("wwwroot/uploads/users", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await model.ProfileImage.CopyToAsync(stream);

                user.ProfileImage = fileName;
            }

            
            await _userRepository.UpdateAsync(user);
        }
    }

}
