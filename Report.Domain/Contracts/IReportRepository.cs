using Domain.Entities.Reports;
using Report.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Report.Domain.Contracts
{
    public interface IReportRepository
    {
        
        Task<List<Report.Domain.Entities.Reports.Report>> GetAllAsync();
        Task AddAsync(Report.Domain.Entities.Reports.Report report);
        Task<Report.Domain.Entities.Reports.Report?> GetByIdAsync(int id);
        Task<List<Report.Domain.Entities.Reports.Report>> GetFilteredAsync(string? search, int? type, int? status, DateTime? fromDate, DateTime? toDate);
        Task UpdateAsync(Report.Domain.Entities.Reports.Report report);
        Task DeleteAsync(Report.Domain.Entities.Reports.Report report);
        Task<List<ReportType>> GetAllReportTypeAsync();
        Task<ReportDetailsViewModel> GetDetailsAsync(int reportId);
        Task<List<ReportListItemViewModel>> GetReportsByUserIdAsync(int userId);
    }
}
