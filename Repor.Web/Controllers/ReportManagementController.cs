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

        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> Index(ReportIndexViewModel model)
        {
            var reports = await _service.GetFilteredAsync(
                model.Search,
                model.ReportTypeId,
                model.ReportStatusId,
                model.FromDate,
                model.ToDate
            );

            model.Reports = reports.Select(r => new ReportListItemViewModel
            {
                Id = r.Id,
                Title = r.Title,
                ReportType = r.ReportType.Name,
                UserFullName = r.User.FirstName + " "+ r.User.LastName,
                CreatedDate = r.CreatedDate,
                Status = (Report.Domain.Entities.Enums1.ReportStatus)r.ReportStatusId
            }).ToList();

            return View(model);
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

            var roleId = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if(roleId == "1" || roleId == "2")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("MyReports");
            }
           
        }

        #endregion

        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> MyReports()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var reports = await _service.GetReportsByUserIdAsync(userId);
            var model = new ReportIndexViewModel { Reports = reports }; 
            return View(model);
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
