using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Report.Application.Services.Interfaces;
using Report.Domain.ViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Repor.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRegisterService _userRegisterService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IUserRegisterService service,
            ILogger<AccountController> logger) // اضافه کردن logger
        {
            _userRegisterService = service;
            _logger = logger;
        }

        #region login 
        public IActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userRegisterService.ValidateUser(model.UserNameOrEmail, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "ایمیل یا پسورد اشتباه است");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),

                // 👇 Claim تصویر کاربر
              new Claim("ProfileImage",
                    string.IsNullOrEmpty(user.ProfileImage)
                        ? "/uploads/profile-images/default.png"
                        : user.ProfileImage.StartsWith("/")
                        ? user.ProfileImage
                        : "/" + user.ProfileImage
                        )

            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Index", "Dashboard");
        }

        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // اضافه کردن برای امنیت
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var result = await _userRegisterService.Create(model, model.Email, model.PhoneNumber);

                if (result)
                {
                    TempData["SuccessMessage"] = "ثبت‌نام با موفقیت انجام شد";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("email", "ایمیل قبلاً ثبت شده است");
                    ModelState.AddModelError("PhoneNumber", "این شماره موبایل قبلاً ثبت شده ");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // لاگ کردن خطا
                _logger.LogError(ex, "خطا در ثبت‌نام کاربر");
                ModelState.AddModelError("", "خطایی در ثبت‌نام رخ داده است");
                return View(model);
            }
        }
        #endregion

        #region Logout
        
          [HttpPost]
            public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }
        
        #endregion
    }
}