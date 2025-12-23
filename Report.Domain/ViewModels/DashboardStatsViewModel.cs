using System;
using System.Collections.Generic;

namespace Report.Domain.ViewModels
{
    public class DashboardStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalReports { get; set; }
        public int PendingReports { get; set; }
        public int ApprovedReports { get; set; }
        public int RejectedReports { get; set; }

        public List<LastReportDto> LastReports { get; set; }
     
    }

    public class LastReportDto
    {
        public string Title { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string ReportType { get; set; }
    }

}
