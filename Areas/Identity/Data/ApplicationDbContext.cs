using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Student_Admission_System.Models;

namespace Student_Admission_System.Areas.Identity.Data
{
    public class ApplicationDbContext2 : IdentityDbContext
    {
        public ApplicationDbContext2(DbContextOptions<ApplicationDbContext2> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<AdmissionStatus> AdmissionStatuses { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // important for Identity tables

            // Student -> AdmissionStatus (One-to-Many)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.AdmissionStatus)
                .WithMany(st => st.Students)
                .HasForeignKey(s => s.AdmissionStatusID);  // FIXED

            // AuditLog -> Student (Many-to-One)
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentID);

            // AuditLog -> Admin (Many-to-One)
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Admin)
                .WithMany()
                .HasForeignKey(a => a.AdminID);
        }
    }
}
