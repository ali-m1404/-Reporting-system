using Domain.Entities.Users;
using Report.Domain.Contracts;
using Report.Domain.Entities.Reports;
using Report.Domain.ViewModels;
using Report.Infrastructure.Repositories;

namespace Report.Services
{

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        private readonly IReportRepository _reportRepository;

        public DashboardService(IDashboardRepository repository, IReportRepository reportRepository)
        {
            _repository = repository;
            _reportRepository = reportRepository;
        }

        // داشبورد سوپرادمین
        public async Task<DashboardStatsViewModel> GetSuperAdminDashboard()
        {
            var reports = await _repository.GetLastReportsAsync();
            var vm = new DashboardStatsViewModel
            {
                TotalUsers = await _repository.GetTotalUsersAsync(),
                TotalReports = await _repository.GetTotalReportsAsync(),
                PendingReports = await _repository.GetReportsCountByStatusAsync("Pending"),
                ApprovedReports = await _repository.GetReportsCountByStatusAsync("Approved"),
                RejectedReports = await _repository.GetReportsCountByStatusAsync("Rejected"),
                LastReports = reports.Select(r => new LastReportDto
                {
                    Title = r.Title,
                    UserName = r.User.FirstName + " " + r.User.LastName,
                    ReportType = r._ReportStatus.Name,
                    CreatedDate = r.CreatedDate,
                    Status = r._ReportStatus.Name
                }).ToList()
            };

            return vm;
        }

        // داشبورد ادمین
        public async Task<DashboardStatsViewModel> GetAdminDashboard(int adminId)
        {
            var reports = await _repository.GetLastReportsAsync(adminId); // لیست گزارش‌ها

            var vm = new DashboardStatsViewModel
            {
                TotalUsers = await _repository.GetUsersByAdminAsync(),
                TotalReports = await _repository.GetTotalReportsAsync(),
                PendingReports = await _repository.GetReportsCountByStatusAsync("Pending"),
                ApprovedReports = await _repository.GetReportsCountByStatusAsync("Approved"),
                RejectedReports = await _repository.GetReportsCountByStatusAsync("Rejected"),

                LastReports = reports.Select(r => new LastReportDto
                {
                    Title = r.Title,
                    UserName = r.User.FirstName + " " + r.User.LastName,
                    ReportType = r._ReportStatus.Name,
                    CreatedDate = r.CreatedDate,
                    Status = r._ReportStatus.Name
                }).ToList()
            };

            return vm;
        }



        // داشبورد کاربر ساده
        public async Task<DashboardStatsViewModel> GetUserDashboard(int userId)
        {
            var reports = await _repository.GetUserReportsAsync(userId);

            var vm = new DashboardStatsViewModel
            {
                TotalUsers = 1,
                TotalReports = reports.Count,

                PendingReports = reports.Count(r => r._ReportStatus.Name == "Pending"),    
                ApprovedReports = reports.Count(r => r._ReportStatus.Name == "Approved"),  
                RejectedReports = reports.Count(r => r._ReportStatus.Name == "Rejected"),  

                LastReports = reports.Select(r => new LastReportDto
                {
                    Title = r.Title,
                    UserName = r.User.FirstName + " " + r.User.LastName,
                    ReportType = r._ReportStatus.Name,
                    CreatedDate = r.CreatedDate,
                    Status = r._ReportStatus.Name     
                }).ToList()
            };

            return vm;
        }

        public async Task<List<ReportListItemViewModel>> GetReportsByRoleAsync(string role, int userId)
        {
            List<Report.Domain.Entities.Reports.Report> reportEntities;

            if (role == "1" || role == "2")
            {
                reportEntities = await _reportRepository.GetAllAsync();
            }
            else
            {
                return await _reportRepository.GetReportsByUserIdAsync(userId);
            }

            // تبدیل به ViewModel
            var result = reportEntities.Select(r => new ReportListItemViewModel
            {
                Id = r.Id,
                Title = r.Title,
                ReportType = r.ReportType.Name,
                CreatedDate = r.CreatedDate,
                Status = r._ReportStatus.Name switch
                {
                    "Pending" => Report.Domain.Entities.Enums1.ReportStatus.Pending,
                    "Approved" => Report.Domain.Entities.Enums1.ReportStatus.Approved,
                    "Rejected" => Report.Domain.Entities.Enums1.ReportStatus.Rejected,
                    _ => throw new ArgumentOutOfRangeException()
                },
                UserFullName = r.User.FirstName + " " + r.User.LastName
            }).ToList();

            return result;
        }

        
    }
}
