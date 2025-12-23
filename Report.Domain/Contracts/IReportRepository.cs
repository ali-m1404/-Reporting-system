using Domain.Entities.Reports;
using Report.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public interface IReportRepository
    {
        
        Task<List<Report.Domain.Entities.Reports.Report>> GetAllAsync();
        Task AddAsync(Report.Domain.Entities.Reports.Report report);
        Task<Report.Domain.Entities.Reports.Report?> GetByIdAsync(int id);
        Task<List<Report.Domain.Entities.Reports.Report>> GetFilteredAsync(ReportFilterViewModel filter);
        Task UpdateAsync(Report.Domain.Entities.Reports.Report report);
        Task DeleteAsync(Report.Domain.Entities.Reports.Report report);
        Task<List<ReportType>> GetAllReportTypeAsync();
        Task<ReportDetailsViewModel> GetDetailsAsync(int reportId);
        Task<List<ReportListItemViewModel>> GetReportsByUserIdAsync(int userId);
    }
}
