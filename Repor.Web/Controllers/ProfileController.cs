using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report.Application.Services.Interfaces;
using Report.Domain.ViewModels;
using System.Security.Claims;

namespace Repor.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // صفحه ویرایش پروفایل
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var model = await _profileService.GetProfileAsync(userId);
            return View(model);
        }

        // ذخیره تغییرات
        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _profileService.UpdateProfileAsync(model);

                TempData["Success"] = "پروفایل با موفقیت بروزرسانی شد";
                return RedirectToAction("Index", "UserManagement");
            }
            catch (Exception ex)
            {
                // پیام خطاهایی که از Service می‌آیند
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
        }

    }

}
