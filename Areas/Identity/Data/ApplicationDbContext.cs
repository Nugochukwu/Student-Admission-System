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

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.ApplicationNumber)
                .IsUnique();
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentID)
                .OnDelete(DeleteBehavior.Cascade);  // logs go if student goes

            modelBuilder.Entity<AdmissionStatus>().HasData
                (
                    new AdmissionStatus { AdmissionStatusID = 1, Name = "PENDING"},
                    new AdmissionStatus { AdmissionStatusID = 2, Name = "ACCEPTED" },
                    new AdmissionStatus { AdmissionStatusID = 3, Name = "REJECTED" }
                );
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Student>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastUpdated = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }

    }
}
