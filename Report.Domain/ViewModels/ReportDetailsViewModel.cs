using System;

namespace Report.Domain.ViewModels
{
    public class ReportDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? ReportTypeId { get; set; }

        public string FilePath { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; } // اضافه شد

        public string ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        // وضعیت عددی
        public int Status
        {
            get
            {
                if (IsApproved) return 2; // تایید شده
                if (IsRejected) return 3; // رد شده
                return 1; // در انتظار
            }
        }
    }
}
