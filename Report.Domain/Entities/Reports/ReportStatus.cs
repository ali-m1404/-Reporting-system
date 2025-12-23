using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Reports
{
    public class ReportStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        // Navigation Properties
        public virtual ICollection<Report.Domain.Entities.Reports.Report> Reports { get; set; }
    }
}
