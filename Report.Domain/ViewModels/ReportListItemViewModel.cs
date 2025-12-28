using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.ViewModels
{
    public class ReportListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ReportType { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Report.Domain.Entities.Enums1.ReportStatus Status { get; set; }
    }


}
