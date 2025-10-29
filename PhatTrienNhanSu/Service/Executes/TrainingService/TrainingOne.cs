using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.Service.TrainingService.Models;

namespace PhatTrienNhanSu.Service.TrainingService
{
    // --- INTERFACE ---
    public interface ITrainingOne
    {
        Task<ProgramDetail?> GetProgramDetailsByIdAsync(int programId);
    }

    // --- IMPLEMENTATION ---
    public class TrainingOne : ITrainingOne
    {
        private readonly PhatTrienNhanSuDbContext _context;
        public TrainingOne(PhatTrienNhanSuDbContext context) { _context = context; }

        public async Task<ProgramDetail?> GetProgramDetailsByIdAsync(int programId)
        {
            return await _context.TrainingPrograms
                .Include(p => p.TrainingCourses)
                .Where(p => p.ProgramID == programId && p.Status == 1)
                .Select(p => new ProgramDetail
                {
                    ProgramID = p.ProgramID,
                    ProgramName = p.ProgramName,
                    Description = p.Description,
                    AvailableCourses = p.TrainingCourses
                        .Where(c => c.Status == 1) // Chỉ lấy khóa học Sắp diễn ra
                        .Select(c => new CourseDetail
                        {
                            CourseID = c.CourseID,
                            CourseName = c.CourseName,
                            Instructor = c.Instructor,
                            Location = c.Location,
                            StartDate = c.StartDate
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}