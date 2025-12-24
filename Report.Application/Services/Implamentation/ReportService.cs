using Domain.Entities.Reports;
using Domain.Entities.Users;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Report.Application.Services.Interfaces;
using Report.Domain.Contracts;
using Report.Domain.ViewModels;
using Report.Infrastructure.Repositories;
using System.IO;



namespace Report.Application.Services.Implamentation
{
    public class ReportService : IReportService
    {
        private IReportRepository _repository;
       

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }
       

     

    public async Task CreateAsync(CreateReportViewModel model)
    {
        // نام یکتای فایل
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);

        // مسیر ریشه wwwroot بدون IWebHostEnvironment
        var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        // مسیر پوشه ذخیره
        var folder = Path.Combine(
            "uploads",
            "reports",
            model.UserId.ToString(),
            DateTime.UtcNow.ToString("yyyy"),
            DateTime.UtcNow.ToString("MM")
        );

        var path = Path.Combine(webRootPath, folder);

        // ساخت پوشه در صورت عدم وجود
        Directory.CreateDirectory(path);

        // مسیر کامل فایل
        var fullPath = Path.Combine(path, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await model.File.CopyToAsync(stream);
        }

        // ذخیره اطلاعات گزارش در دیتابیس
        var report = new Report.Domain.Entities.Reports.Report
        {

            UserId = model.UserId,
            Title = model.Title,
           
            Description = model.Description,
            FilePath = "/" + Path.Combine(folder, fileName).Replace("\\", "/"),
           
            ReportTypeId = model.ReportTypeId,
            ReportStatusId = 1 // Pending
        };

        await _repository.AddAsync(report);
    }


        #region ActionMethods

        public async Task<ReportDetailsViewModel> GetDetailsAsync(int reportId)
        {
            return await _repository.GetDetailsAsync(reportId);
        }


        public async Task ApproveAsync(int id, int adminId)
        {
            var report = await _repository.GetByIdAsync(id);
            report.ReportStatusId = 2;
            report.ApprovedById = adminId;
            report.UpdatedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(report);
        }

        public async Task RejectAsync(int reportId, int adminId)
        {
            var report = await _repository.GetByIdAsync(reportId);
            report.ReportStatusId = 3;
            report.ApprovedById = adminId;
            report.UpdatedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(report); ;
        }
        public async Task DeleteAsync(int reportId)
        {
            var report = await _repository.GetByIdAsync(reportId);

            if (report == null)
                throw new Exception("گزازش یافت نه شد ");
            await _repository.DeleteAsync(report);


        }


        #endregion


        public async Task<List<ReportListItemViewModel>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();

            return reports.Select(r => new ReportListItemViewModel
            {
                Id = r.Id,
                Title = r.Title,
                ReportType = r.ReportType.Name,
                UserFullName = r.User.FirstName + " " + r.User.LastName,
                CreatedDate = r.CreatedDate,
               Status = (Domain.Entities.Enums1.ReportStatus)r.ReportStatusId,
            }).ToList();
        }

  

        public async Task<CreateReportViewModel> GetCreateReportModelAsync()
            {
                var types = await _repository.GetAllReportTypeAsync();

                return new CreateReportViewModel
                {
                    ReportTypes = types.Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    }).ToList()
                };
            }

    public async Task<List<Domain.Entities.Reports.Report>> GetFilteredAsync(
           ReportFilterViewModel filter)
        {
            // 🛡 اعتبارسنجی اولیه
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            // 🛡 اصلاح بازه تاریخ (خطای رایج)
            if (filter.FromDate.HasValue && filter.ToDate.HasValue)
            {
                if (filter.FromDate > filter.ToDate)
                    throw new Exception("تاریخ شروع نمی‌تواند بزرگتر از تاریخ پایان باشد");
            }

            // 🔹 دریافت داده‌ها از Repository
            var reports = await _repository.GetFilteredAsync(filter);

            return reports;
        }

        public async Task<List<ReportListItemViewModel>> GetReportsByUserIdAsync(int userId)
        {
            return await _repository.GetReportsByUserIdAsync(userId);
        }
    }
}
