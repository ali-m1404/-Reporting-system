using Domain.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public interface IDashboardRepository
    {
       
        /// تعداد کل کاربران سیستم
        
        Task<int> GetTotalUsersAsync();

        /// <summary>
        /// تعداد کاربران ساده (User) برای ادمین
        /// </summary>
        Task<int> GetUsersByAdminAsync();

        /// <summary>
        /// تعداد کل گزارشات
        /// </summary>
        Task<int> GetTotalReportsAsync();

        /// <summary>
        /// تعداد گزارشات بر اساس وضعیت (Pending, Approved, Rejected)
        /// </summary>
        /// <param name="status">وضعیت گزارش</param>
        Task<int> GetReportsCountByStatusAsync(string status);

        /// <summary>
        /// دریافت آخرین گزارش‌ها (به صورت پیش‌فرض ۵ گزارش)
        /// </summary>
        /// <param name="count">تعداد گزارش‌ها</param>
        Task<List<Report.Domain.Entities.Reports.Report>> GetLastReportsAsync(int count = 5);

        ///// <summary>
        ///// دریافت گزارشات کاربر خاص
        ///// </summary>
        ///// <param name="userId">شناسه کاربر</param>
        Task<List<Report.Domain.Entities.Reports.Report>> GetUserReportsAsync(int userId);
    }
}
