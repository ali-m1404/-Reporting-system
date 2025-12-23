using Data.Context;
using Domain.Entities.Reports;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;

namespace Report.Data.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ReportingSystemDbContext _context;

        public DashboardRepository(ReportingSystemDbContext context)
        {
            _context = context;
        }

        // آمار کل کاربران
        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        // آمار کاربران ساده برای Admin
        public async Task<int> GetUsersByAdminAsync()
        {
            return await _context.Users.Where(u => u.RoleId == 3).CountAsync();
        }

        // آمار کل گزارشات
        public async Task<int> GetTotalReportsAsync()
        {
            return await _context.Reports.CountAsync();
        }

        // آمار گزارشات بر اساس وضعیت
        public async Task<int> GetReportsCountByStatusAsync(string status)
        {
            if (!Enum.TryParse<Report.Domain.Entities.Enums1.ReportStatus>(
                    status, true, out var enumStatus))
                return 0;

            int statusId = (int)enumStatus;

            return await _context.Reports
                .CountAsync(r => r.ReportStatusId == statusId);
        }


        // آخرین گزارش‌ها
        public async Task<List<Report.Domain.Entities.Reports.Report>> GetLastReportsAsync(int count = 9)
        {
            return await _context.Reports
                .Include(r => r.User)
                .Include(r => r._ReportStatus)
                .OrderByDescending(r => r.CreatedDate)
                .Take(5)
                .ToListAsync();
        }
        // گزارشات کاربر ساده
        public async Task<List<Report.Domain.Entities.Reports.Report>> GetUserReportsAsync(int userId)
        {
            return await _context.Reports
                .Include(r => r.User)
                .Include(r => r._ReportStatus) // ← وضعیت را Include کنید
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }



        //Task<List<Domain.Entities.Reports.Report>> IDashboardRepository.GetUserReportsAsync(int userId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
