using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PhatTrienNhanSu.DbContexts
{
    // Đổi tên DbContext thành một tên có ý nghĩa hơn, ví dụ PhatTrienNhanSuDbContext
    public class PhatTrienNhanSuDbContext : DbContext
    {
        public PhatTrienNhanSuDbContext(DbContextOptions<PhatTrienNhanSuDbContext> options) : base(options) { }

        #region MODULE KHẢO SÁT
        public DbSet<SurveyPeriod> SurveyPeriods { get; set; }
        public DbSet<CourseCatalog> CourseCatalog { get; set; }
        public DbSet<EmployeeSurveyResponse> EmployeeSurveyResponses { get; set; }
        #endregion

        #region MODULE ĐÀO TẠO THỰC TẾ
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<TrainingCourse> TrainingCourses { get; set; }
        public DbSet<EmployeeTrainingRegistration> EmployeeTrainingRegistrations { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình các ràng buộc UNIQUE (Unique Constraints)
            modelBuilder.Entity<CourseCatalog>()
                .HasIndex(c => c.CourseCode)
                .IsUnique();

            modelBuilder.Entity<TrainingProgram>()
                .HasIndex(p => p.ProgramCode)
                .IsUnique();

            modelBuilder.Entity<EmployeeSurveyResponse>()
                .HasIndex(r => new { r.EmployeeID, r.CatalogID, r.SurveyPeriodId })
                .IsUnique()
                .HasDatabaseName("IX_Unique_Employee_Survey_Item"); // Đặt tên rõ ràng cho constraint

            modelBuilder.Entity<EmployeeTrainingRegistration>()
                .HasIndex(r => new { r.EmployeeID, r.CourseID })
                .IsUnique()
                .HasDatabaseName("IX_Unique_Employee_Course_Registration");

            // Cấu hình giá trị mặc định cho các cột ngày tạo
            modelBuilder.Entity<SurveyPeriod>().Property(p => p.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<TrainingProgram>().Property(p => p.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<TrainingCourse>().Property(p => p.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<EmployeeTrainingRegistration>().Property(p => p.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<EmployeeTrainingRegistration>().Property(p => p.RegisterDate).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<EmployeeSurveyResponse>().Property(p => p.SubmissionDate).HasDefaultValueSql("GETUTCDATE()");

            // Cấu hình hành vi xóa chuỗi (Cascading Deletes)
            // Khi xóa 1 Program, các Course con sẽ bị xóa theo.
            modelBuilder.Entity<TrainingProgram>()
                .HasMany(p => p.TrainingCourses)
                .WithOne(c => c.TrainingProgram)
                .HasForeignKey(c => c.ProgramID)
                .OnDelete(DeleteBehavior.Cascade);

            // Khi xóa 1 Course, các Registration liên quan cũng bị xóa theo.
            modelBuilder.Entity<TrainingCourse>()
                .HasMany(c => c.Registrations)
                .WithOne(r => r.TrainingCourse)
                .HasForeignKey(r => r.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan trọng: Ngăn chặn lỗi "multiple cascade paths"
            // Khi 1 Program bị xóa, các Registration liên quan KHÔNG tự động xóa trực tiếp
            // mà sẽ được xóa gián tiếp thông qua việc xóa Course.
            modelBuilder.Entity<TrainingProgram>()
                .HasMany(p => p.Registrations)
                .WithOne(r => r.TrainingProgram)
                .HasForeignKey(r => r.ProgramID)
                .OnDelete(DeleteBehavior.NoAction);

            // Tương tự cho module khảo sát
            modelBuilder.Entity<SurveyPeriod>()
               .HasMany(p => p.SurveyResponses)
               .WithOne(r => r.SurveyPeriod)
               .HasForeignKey(r => r.SurveyPeriodId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseCatalog>()
                .HasMany(c => c.SurveyResponses)
                .WithOne(r => r.CourseCatalog)
                .HasForeignKey(r => r.CatalogID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}