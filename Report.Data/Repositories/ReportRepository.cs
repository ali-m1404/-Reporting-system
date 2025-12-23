using Data.Context;
using Domain.Entities.Reports;
using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;
using Report.Domain.Entities.Reports;
using Report.Domain.ViewModels;
using System;

namespace Report.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportingSystemDbContext _context;

        public ReportRepository(ReportingSystemDbContext context)
        {
            _context = context;
        }

        // ➕ Create
        public async Task AddAsync(Report.Domain.Entities.Reports.Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        // 🔍 Get By Id
        public async Task<Report.Domain.Entities.Reports.Report?> GetByIdAsync(int id)
        {
            return await _context.Reports
                .Include(r => r.User)
                .Include(r => r.ReportType)
                .Include(r => r._ReportStatus)
                .Include(r => r.ApprovedBy)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // 🔍 Filter
        public async Task<List<Report.Domain.Entities.Reports.Report>> GetFilteredAsync(ReportFilterViewModel filter)
        {
            var query = _context.Reports
                //.Include(r => r.UserId)
                .Include(r => r.ReportType)
                .Include(r => r._ReportStatus)
                .AsQueryable();

            if (filter.ReportTypeId.HasValue)
                query = query.Where(r => r.ReportTypeId == filter.ReportTypeId);

            if (filter.ReportStatusId.HasValue)
                query = query.Where(r => r.ReportStatusId == filter.ReportStatusId);

            if (filter.FromDate.HasValue)
                query = query.Where(r => r.CreatedDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(r => r.CreatedDate <= filter.ToDate.Value);

            return await query
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }

        // ✏ Update
        public async Task UpdateAsync(Report.Domain.Entities.Reports.Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }

        // ❌ Delete
        public async Task DeleteAsync(Report.Domain.Entities.Reports.Report report)
        {
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Report.Domain.Entities.Reports.Report> >GetAllAsync()
        {
            return await _context.Reports
                .Include(r => r.User)
                .Include(r => r.ReportType)
                .Include(r => r._ReportStatus)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<ReportType>> GetAllReportTypeAsync()
        {
            return await _context.ReportTypes
           .OrderBy(x => x.Name)
           .ToListAsync();
        }


        public async Task<ReportDetailsViewModel> GetDetailsAsync(int reportId)
        {
            return await _context.Reports
                .Where(r => r.Id == reportId)
                .Select(r => new ReportDetailsViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    ReportType = r.ReportType.Name,
                    FilePath = r.FilePath,
                    CreatedBy = r.User.FirstName,
                    CreatedAt = r.CreatedDate,
                    IsApproved= r.ApprovedById != null && r.ApprovedById > 0
,
                    ApprovedBy = r.ApprovedBy != null ? r.ApprovedBy.FirstName: "-",
                    ApprovedAt = r.UpdatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ReportListItemViewModel>> GetReportsByUserIdAsync(int userId)
        {
            return await _context.Reports
                .Where(r => r.UserId == userId)
                .Select(r => new ReportListItemViewModel
                {
                    Id = r.Id,
                    UserFullName = r.User.FirstName + "-" + r.User.LastName,
                    Title = r.Title,
                    ReportType = r.ReportType.Name,
                    CreatedDate = r.CreatedDate,
                    Status = (Report.Domain.Entities.Enums1.ReportStatus)r.ReportStatusId,
                })
                .ToListAsync();
        }
    }
}
