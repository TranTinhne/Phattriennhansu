using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.DbContexts.Entities;
using PhatTrienNhanSu.Service.Executes.SurveyService.Models;

namespace PhatTrienNhanSu.Service.Executes.SurveyService
{
    // --- INTERFACE ---
    public interface ISurveyMany
    {
        /// Lấy danh sách các mục catalog đang hoạt động, được nhóm theo lĩnh vực.
        Task<List<SurveyAreaGroup>> GetGroupedCatalogItemsAsync();

        /// Lấy danh sách các đối tượng SurveySelection rỗng để form có thể binding dữ liệu.
        Task<List<SurveySelection>> GetInitialSelectionsAsync();

        /// Lấy danh sách các Nhà cung cấp đang hoạt động để điền vào dropdown.
        Task<IEnumerable<TrainingProvider>> GetActiveProvidersAsync();
    }

    // --- IMPLEMENTATION ---
    public class SurveyMany : ISurveyMany
    {
        private readonly PhatTrienNhanSuDbContext _context;
        public SurveyMany(PhatTrienNhanSuDbContext context) { _context = context; }

        public async Task<List<SurveyAreaGroup>> GetGroupedCatalogItemsAsync()
        {
            return await _context.CourseCatalog
                .Include(c => c.Provider) // Join với bảng Provider
                .OrderBy(c => c.Area).ThenBy(c => c.CourseCode)
                .GroupBy(c => c.Area)
                .Select(g => new SurveyAreaGroup
                {
                    AreaName = g.Key ?? "Lĩnh vực khác",
                    CatalogItems = g.Select(c => new CatalogItem
                    {
                        Id = c.Id,
                        CourseCode = c.CourseCode,
                        CourseName = c.CourseName,
                        DurationHours = c.DurationHours,
                        Provider = c.Provider != null ? c.Provider.ProviderName : "Chưa có" // Lấy tên Provider
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<SurveySelection>> GetInitialSelectionsAsync()
        {
            return await _context.CourseCatalog
                .Where(c => c.Status == 1)
                .Select(c => new SurveySelection { CatalogID = c.Id })
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainingProvider>> GetActiveProvidersAsync()
        {
            return await _context.TrainingProviders
                .Where(p => p.Status == 1)
                .OrderBy(p => p.ProviderName)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}