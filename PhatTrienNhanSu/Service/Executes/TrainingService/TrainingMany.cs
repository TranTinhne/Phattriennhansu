using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.Service.TrainingService.Models;

namespace PhatTrienNhanSu.Service.TrainingService
{
    // --- INTERFACE ---
    public interface ITrainingMany
    {
        Task<IEnumerable<ProgramSummary>> GetActiveProgramsAsync();
        Task<IEnumerable<RegistrationDetail>> GetMyRegistrationsAsync(int employeeId);
    }

    // --- IMPLEMENTATION ---
    public class TrainingMany : ITrainingMany
    {
        private readonly PhatTrienNhanSuDbContext _context;

        public TrainingMany(PhatTrienNhanSuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgramSummary>> GetActiveProgramsAsync()
        {
            return await _context.TrainingPrograms
                .Where(p => p.Status == 1) // Chỉ lấy các chương trình có Status = 1 (Active)
                .OrderByDescending(p => p.StartDate)
                .Select(p => new ProgramSummary
                {
                    ProgramID = p.ProgramID,
                    ProgramCode = p.ProgramCode,
                    ProgramName = p.ProgramName,
                    StartDate = p.StartDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<RegistrationDetail>> GetMyRegistrationsAsync(int employeeId)
        {
            return await _context.EmployeeTrainingRegistrations
                .Where(r => r.EmployeeID == employeeId)
                .Include(r => r.TrainingProgram)
                .Include(r => r.TrainingCourse)
                .OrderByDescending(r => r.RegisterDate)
                .Select(r => new RegistrationDetail
                {
                    ProgramName = r.TrainingProgram.ProgramName,
                    CourseName = r.TrainingCourse.CourseName,
                    RegisterDate = r.RegisterDate,
                    Status = ConvertRegistrationStatusToString(r.Status),
                    Score = r.Score,
                    Certificate = r.Certificate
                })
                .ToListAsync();
        }

        // Hàm tiện ích để chuyển đổi Status từ số sang chuỗi
        private static string ConvertRegistrationStatusToString(sbyte status)
        {
            return status switch
            {
                -1 => "Đã hủy",
                0 => "Chờ phê duyệt",
                1 => "Đã phê duyệt",
                2 => "Bị từ chối",
                3 => "Đã hoàn thành",
                _ => "Không xác định"
            };
        }
    }
}