using Domain.Entities;
using Domain.Entities.Reports;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;





namespace Data.Context
{
    public class ReportingSystemDbContext: DbContext
    {

      
        public ReportingSystemDbContext(DbContextOptions<ReportingSystemDbContext> options): base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Report.Domain.Entities.Reports.Report> Reports { get; set; }


        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<ReportStatus> ReportStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // رابطه User → Reports (Report.UserId)
            modelBuilder.Entity<Report.Domain.Entities.Reports.Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // رابطه User → ApprovedReports (Report.ApprovedById)
            modelBuilder.Entity<Report.Domain.Entities.Reports.Report>()
                .HasOne(r => r.ApprovedBy)
                .WithMany(u => u.ApprovedReports)
                .HasForeignKey(r => r.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
