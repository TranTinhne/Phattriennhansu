using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.Service.Executes.ProviderService.Models;

namespace PhatTrienNhanSu.Service.Executes.ProviderService
{
    // --- INTERFACE ---
    public interface IProviderMany
    {
        /// Lấy danh sách tất cả các nhà cung cấp trong hệ thống.
        Task<IEnumerable<ProviderDto>> GetAllProvidersAsync();
    }

    // --- IMPLEMENTATION ---
    public class ProviderMany : IProviderMany
    {
        private readonly PhatTrienNhanSuDbContext _context;

        public ProviderMany(PhatTrienNhanSuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProviderDto>> GetAllProvidersAsync()
        {
            return await _context.TrainingProviders
                .AsNoTracking() // Tối ưu hiệu năng vì đây là truy vấn chỉ đọc
                .OrderBy(p => p.ProviderName)
                .Select(p => new ProviderDto
                {
                    Id = p.Id,
                    ProviderCode = p.ProviderCode,
                    ProviderName = p.ProviderName,
                    IsInternal = p.IsInternal,
                    Status = p.Status
                })
                .ToListAsync();
        }
    }
}