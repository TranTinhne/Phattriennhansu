using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.DbContexts.Entities;

namespace PhatTrienNhanSu.Service.Executes.SurveyService
{
    // --- INTERFACE ---
    public interface ISurveyOne
    {
        /// Lấy thông tin về kỳ khảo sát đang hoạt động.
        Task<SurveyPeriod?> GetActiveSurveyPeriodAsync();

        /// Kiểm tra xem một nhân viên đã nộp khảo sát cho một kỳ cụ thể chưa.
        Task<bool> HasEmployeeSubmittedSurveyAsync(int employeeId, int surveyPeriodId);

        /// Lấy thông tin chi tiết của một mục trong danh mục khóa học theo ID.
        Task<CourseCatalog?> GetCatalogItemByIdAsync(int id);
    }

    // --- IMPLEMENTATION ---
    public class SurveyOne : ISurveyOne
    {
        private readonly PhatTrienNhanSuDbContext _context;
        public SurveyOne(PhatTrienNhanSuDbContext context) { _context = context; }

        public async Task<SurveyPeriod?> GetActiveSurveyPeriodAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.SurveyPeriods
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Status == 1 && p.StartDate <= now && p.EndDate >= now);
        }

        public async Task<bool> HasEmployeeSubmittedSurveyAsync(int employeeId, int surveyPeriodId)
        {
            return await _context.EmployeeSurveyResponses
                .AsNoTracking()
                .AnyAsync(r => r.EmployeeID == employeeId && r.SurveyPeriodId == surveyPeriodId);
        }

        public async Task<CourseCatalog?> GetCatalogItemByIdAsync(int id)
        {
            // Dùng FindAsync để tìm theo khóa chính, hiệu năng cao.
            return await _context.CourseCatalog.FindAsync(id);
        }
    }
}