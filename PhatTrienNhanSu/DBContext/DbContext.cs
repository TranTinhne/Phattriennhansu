using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts.Entities;
using PhatTrienNhanSu.Service.Interfaces;

namespace PhatTrienNhanSu.DbContexts
{
    public class PhatTrienNhanSuDbContext : DbContext
    {
        private readonly IUserAccessor _userAccessor;

        // Inject IUserAccessor vào DbContext
        public PhatTrienNhanSuDbContext(DbContextOptions<PhatTrienNhanSuDbContext> options, IUserAccessor userAccessor) : base(options)
        {
            _userAccessor = userAccessor;
        }

        public DbSet<SurveyPeriod> SurveyPeriods { get; set; }
        public DbSet<CourseCatalog> CourseCatalog { get; set; }
        public DbSet<EmployeeSurveyResponse> EmployeeSurveyResponses { get; set; }
        public DbSet<TrainingProvider> TrainingProviders { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUserId = _userAccessor.GetCurrentUserId();
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUserId;
                        entry.Entity.CreatedDate = now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = currentUserId;
                        entry.Entity.UpdatedDate = now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CourseCatalog>()
                .HasIndex(c => c.CourseCode)
                .IsUnique();

            modelBuilder.Entity<EmployeeSurveyResponse>()
                .HasIndex(r => new { r.EmployeeID, r.CatalogID, r.SurveyPeriodId })
                .IsUnique();

            modelBuilder.Entity<TrainingProvider>()
                .HasIndex(p => p.ProviderCode)
                .IsUnique();
        }
    }
}