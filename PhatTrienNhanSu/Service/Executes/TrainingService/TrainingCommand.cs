using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.DbContexts.Entities;
using PhatTrienNhanSu.Service.Results;

namespace PhatTrienNhanSu.Service.TrainingService
{
    // --- INTERFACE ---
    public interface ITrainingCommand
    {
        Task<ServiceResult> RegisterForCourseAsync(int employeeId, int courseId);
    }

    // --- IMPLEMENTATION ---
    public class TrainingCommand : ITrainingCommand
    {
        private readonly PhatTrienNhanSuDbContext _context;
        public TrainingCommand(PhatTrienNhanSuDbContext context) { _context = context; }

        public async Task<ServiceResult> RegisterForCourseAsync(int employeeId, int courseId)
        {
            // Kiểm tra 1: Khóa học có tồn tại và đang mở để đăng ký không?
            var course = await _context.TrainingCourses
                .FirstOrDefaultAsync(c => c.CourseID == courseId && c.Status == 1);

            if (course == null)
            {
                return ServiceResult.Fail("Khóa học không hợp lệ hoặc đã hết hạn đăng ký.");
            }

            // Kiểm tra 2: Nhân viên này đã đăng ký khóa học này trước đó chưa?
            // (Chỉ kiểm tra các đăng ký chưa bị hủy)
            var isAlreadyRegistered = await _context.EmployeeTrainingRegistrations
                .AnyAsync(r => r.EmployeeID == employeeId && r.CourseID == courseId && r.Status != -1);

            if (isAlreadyRegistered)
            {
                return ServiceResult.Fail("Bạn đã đăng ký khóa học này rồi.");
            }

            // Kiểm tra 3: Khóa học còn chỗ trống không?
            var currentRegistrations = await _context.EmployeeTrainingRegistrations
                .CountAsync(r => r.CourseID == courseId && r.Status >= 0); // Đếm các đăng ký không bị hủy

            if (currentRegistrations >= course.MaxAttendees)
            {
                return ServiceResult.Fail("Khóa học đã đủ số lượng học viên.");
            }

            var newRegistration = new EmployeeTrainingRegistration
            {
                EmployeeID = employeeId,
                CourseID = courseId,
                ProgramID = course.ProgramID,
                Status = (sbyte)(course.RequiresApproval ? 0 : 1), // 0: Pending, 1: Approved
                Certificate = false,
                CreatedBy = employeeId
            };

            await _context.EmployeeTrainingRegistrations.AddAsync(newRegistration);
            await _context.SaveChangesAsync();

            return ServiceResult.Success();
        }
    }
}