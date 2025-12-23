using Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using MyEshop.Core.Security;
using Report.Application.Services.Interfaces;
using Report.Domain.Contracts;
using Report.Domain.ViewModels;


namespace Report.Application.Services.Implamentation
{

    public class UserRegisterService : IUserRegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;

        public UserRegisterService(IUserRepository userRepository, IFileService fileService)
        {
            _userRepository = userRepository;
            _fileService = fileService;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userRepository.IsEmailExistAsync(email);
        }

        public async Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber)
        {
            
            return await _userRepository.IsPhoneNumberExistsAsync(phoneNumber);
        }

        public async Task<bool> Create(RegisterUserViewModel model, string email, string phoneNumber)
        {
            // بررسی تکراری نبودن
            if (await CheckPhoneNumberExistsAsync(phoneNumber) || await CheckEmailExistsAsync(email))
            {
                return false;
            }

            string profileImagePath = null;
            string identityImagePath = null;

            // ✅ استفاده از فیلدهای IFormFile
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                profileImagePath = await _fileService.SaveFileAsync(
                    model.ProfileImage, // ✅ این IFormFile است
                    "uploads/profile-images");
            }

            if (model.IdentityImage != null && model.IdentityImage.Length > 0)
            {
                identityImagePath = await _fileService.SaveFileAsync(
                    model.IdentityImage, // ✅ این IFormFile است
                    "uploads/identity-images");
            }

            // ایجاد کاربر
            var user = new User()
            {
                CreatedDate = DateTime.Now,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = PasswordHasher.HashPassword(model.Password),
                ProfileImage = profileImagePath, // ذخیره مسیر (string)
                NationalIdImg = identityImagePath, // ذخیره مسیر (string)
            };

            _userRepository.Add(user);
            await _userRepository.SaveAsync();

            return true;
        }

        #region Login 
        public User ValidateUser(string email, string password)
        {

            var user = _userRepository.GetUserByEmail(email);
            if (user == null) return null;

            bool isValid = PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);



            if (!isValid) return null; // اینجا بعداً Hash اضافه می‌کنی

            return user;
        }
        #endregion
    }
}
