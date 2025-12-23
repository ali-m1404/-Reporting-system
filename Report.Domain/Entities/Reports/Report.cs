using Domain.Entities.Reports;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities.Reports
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public string FilePath { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int? ReportTypeId { get; set; }

        [Required]
        public int ReportStatusId { get; set; }

        public int? ApprovedById { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ReportTypeId")]
        public virtual  ReportType ReportType { get; set; }

        [ForeignKey("ReportStatusId")]
        public virtual global::Domain.Entities.Reports.ReportStatus _ReportStatus { get; set; }

        [ForeignKey("ApprovedById")]
        public virtual User ApprovedBy { get; set; }
    }
}
