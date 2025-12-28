using Report.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Report.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<ReportListItemViewModel>> GetAllAsync();
        Task CreateAsync(CreateReportViewModel model);
        Task ApproveAsync(int reportId, int adminId);
        Task RejectAsync(int reportId, int adminId);
        Task DeleteAsync(int reportId);
        Task<ReportDetailsViewModel> GetDetailsAsync(int reportId);
        Task<List<Report.Domain.Entities.Reports.Report>> GetFilteredAsync(string search,int? type, int? status,DateTime? toDate, DateTime? fromDate);
        Task<CreateReportViewModel> GetCreateReportModelAsync();
        Task<List<ReportListItemViewModel>> GetReportsByUserIdAsync(int userId);

    }
}
