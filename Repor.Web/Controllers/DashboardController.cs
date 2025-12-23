using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report.Domain.Contracts;
using System.Security.Claims;

namespace ReportingSystem.Web.Controllers
{
    [Authorize] // همه باید لاگین باشند
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (role == "1")
            {
                var model = await _dashboardService.GetSuperAdminDashboard();
                return View("SuperAdminDashboard", model);
            }
            else if (role == "2")
            {
                var model = await _dashboardService.GetAdminDashboard(userId);
                return View("AdminDashboard", model);
            }
            else
            {
                var model = await _dashboardService.GetUserDashboard(userId);
                return View("UserDashboard", model);
            }
        }

        public async Task<IActionResult> Reports()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var reports = await _dashboardService.GetReportsByRoleAsync(role, userId);

            return View(reports);
        }
    }
}
