using Report.Domain.ViewModels;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public interface IDashboardService
    {
        // داشبورد سوپرادمین
        Task<DashboardStatsViewModel> GetSuperAdminDashboard();

        // داشبورد ادمین
        Task<DashboardStatsViewModel> GetAdminDashboard(int adminId);

        // داشبورد کاربر ساده
        Task<DashboardStatsViewModel> GetUserDashboard(int userId);

        Task<List<ReportListItemViewModel>> GetReportsByRoleAsync(string role, int userId);
    }
}
