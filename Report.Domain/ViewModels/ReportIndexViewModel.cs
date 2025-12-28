using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.ViewModels
{
    public class ReportIndexViewModel
    {
        // فیلترها
        public string? Search { get; set; }
        public int? ReportTypeId { get; set; }
        public int? ReportStatusId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // لیست گزارش‌ها
        public List<ReportListItemViewModel> Reports { get; set; } = new();
    }

}
