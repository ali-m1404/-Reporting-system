using Domain.Entities.Reports;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report.Application.Services.Implamentation;
using Report.Application.Services.Interfaces;
using Report.Domain.ViewModels;
using System.Security.Claims;
using TechTalk.SpecFlow.Assist;

namespace Repor.Web.Controllers
{
    [Authorize(Roles = "1,2,3")]
    public class ReportManagementController : Controller
    {
        private readonly IReportService _service;
        public ReportManagementController(IReportService service) 
        {
            _service = service;
        }

        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Index(ReportFilterViewModel filter)
        {
          

            if (filter != null && (filter.ReportTypeId.HasValue || filter.ReportStatusId.HasValue || filter.FromDate.HasValue || filter.ToDate.HasValue))
            {
                // اگر حداقل یکی از فیلترها پر شده باشد، فیلتر اعمال می‌شود
                 await _service.GetFilteredAsync(filter);
            }
            var reports = await _service.GetAllAsync();
            return View(reports);
        }

        #region CreatReport
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _service.GetCreateReportModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

          

            await _service.CreateAsync(model);

            return RedirectToAction("Index");
        }

        #endregion

        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> MyReports()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var reports = await _service.GetReportsByUserIdAsync(userId);
            return View(reports);
        }




        #region ActionsMethodes
        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _service.GetDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }



        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Approve(int id)
        {
                // 👇 Id یوزر لاگین‌شده
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int approverUserId))
                {
                    return Unauthorized();
                }

                await _service.ApproveAsync(id, approverUserId);
                return RedirectToAction("Index");
        }

        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Reject(int id)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int approverUserId))
            {
                return Unauthorized();
            }
            await _service.RejectAsync(id, approverUserId);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        #endregion

       
    }
}
