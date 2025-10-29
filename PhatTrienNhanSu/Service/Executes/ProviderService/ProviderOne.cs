using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.DbContexts.Entities;

namespace PhatTrienNhanSu.Service.Executes.ProviderService
{
    // --- INTERFACE ---
    public interface IProviderOne
    {

        /// Lấy thông tin chi tiết của một Nhà cung cấp theo ID.
        Task<TrainingProvider?> GetProviderByIdAsync(int id);
    }

    // --- IMPLEMENTATION ---
    public class ProviderOne : IProviderOne
    {
        private readonly PhatTrienNhanSuDbContext _context;

        public ProviderOne(PhatTrienNhanSuDbContext context)
        {
            _context = context;
        }

        public async Task<TrainingProvider?> GetProviderByIdAsync(int id)
        {
            // Dùng FindAsync là cách hiệu quả nhất để tìm một đối tượng theo khóa chính.
            return await _context.TrainingProviders.FindAsync(id);
        }
    }
}