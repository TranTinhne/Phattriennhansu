using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.DbContexts.Entities;
using PhatTrienNhanSu.Service.Executes.SurveyService.Models;
using PhatTrienNhanSu.Service.Results;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PhatTrienNhanSu.Service.Executes.SurveyService
{
    // --- INTERFACE ---
    public interface ISurveyCommand
    {
        // Chức năng cho Nhân viên
        Task<ServiceResult> SubmitSurveyAsync(int employeeId, SurveyFormViewModel submission);

        // Chức năng CRUD cho Admin
        Task<ServiceResult> CreateCatalogItemAsync(CourseCatalog newItem, int creatorId);
        Task<ServiceResult> UpdateCatalogItemAsync(CourseCatalog updatedItem, int updaterId);
        Task<ServiceResult> DeleteCatalogItemAsync(int id, int deleterId);
    }

    // --- IMPLEMENTATION ---
    public class SurveyCommand : ISurveyCommand
    {
        private readonly PhatTrienNhanSuDbContext _context;
        public SurveyCommand(PhatTrienNhanSuDbContext context) { _context = context; }

        #region Chức năng cho Nhân viên
        public async Task<ServiceResult> SubmitSurveyAsync(int employeeId, SurveyFormViewModel submission)
        {
            var now = DateTime.UtcNow;
            var activePeriod = await _context.SurveyPeriods
                .AnyAsync(p => p.Id == submission.SurveyPeriodId && p.Status == 1 && p.StartDate <= now && p.EndDate >= now);
            if (!activePeriod)
                return ServiceResult.Fail("Kỳ khảo sát đã kết thúc hoặc không hợp lệ.");

            var hasSubmitted = await _context.EmployeeSurveyResponses
                .AnyAsync(r => r.EmployeeID == employeeId && r.SurveyPeriodId == submission.SurveyPeriodId);
            if (hasSubmitted)
                return ServiceResult.Fail("Bạn đã nộp khảo sát cho kỳ này rồi.");

            var userChoices = submission.Selections.Where(s => s.IsSelected).ToList();
            if (!userChoices.Any())
                return ServiceResult.Fail("Vui lòng chọn ít nhất một nguyện vọng.");

            var newResponses = userChoices.Select(choice => new EmployeeSurveyResponse
            {
                EmployeeID = employeeId,
                CatalogID = choice.CatalogID,
                SurveyPeriodId = submission.SurveyPeriodId,
                LevelUpdateKnowledge = choice.LevelUpdateKnowledge,
                LevelEnhanceKnowledge = choice.LevelEnhanceKnowledge,
                LevelNecessary = choice.LevelNecessary,
                LevelVeryNecessary = choice.LevelVeryNecessary,
                IsPriority = choice.IsPriority
            });

            await _context.EmployeeSurveyResponses.AddRangeAsync(newResponses);
            await _context.SaveChangesAsync();
            return ServiceResult.Success();
        }
        #endregion

        #region Chức năng CRUD cho Admin
        public async Task<ServiceResult> CreateCatalogItemAsync(CourseCatalog newItem, int creatorId)
        {
            // Kiểm tra trùng lặp mã
            if (await _context.CourseCatalog.AnyAsync(c => c.CourseCode == newItem.CourseCode))
                return ServiceResult.Fail($"Mã khóa học '{newItem.CourseCode}' đã tồn tại.");

            // Gán các giá trị kiểm soát
            newItem.CreatedBy = creatorId;
            // EF Core sẽ tự động điền CreatedDate nếu đã cấu hình SaveChangesAsync()

            _context.CourseCatalog.Add(newItem);
            await _context.SaveChangesAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UpdateCatalogItemAsync(CourseCatalog updatedItem, int updaterId)
        {
            var existingItem = await _context.CourseCatalog.FindAsync(updatedItem.Id);
            if (existingItem == null)
                return ServiceResult.Fail("Không tìm thấy khóa học để cập nhật.");

            // Kiểm tra trùng lặp mã (trừ chính nó)
            if (await _context.CourseCatalog.AnyAsync(c => c.CourseCode == updatedItem.CourseCode && c.Id != updatedItem.Id))
                return ServiceResult.Fail($"Mã khóa học '{updatedItem.CourseCode}' đã được sử dụng bởi một khóa học khác.");

            // Cập nhật các trường dữ liệu
            existingItem.CourseCode = updatedItem.CourseCode;
            existingItem.CourseName = updatedItem.CourseName;
            existingItem.Area = updatedItem.Area;
            existingItem.DurationHours = updatedItem.DurationHours;
            existingItem.ProviderID = updatedItem.ProviderID;
            existingItem.Status = updatedItem.Status;

            // Cập nhật các trường kiểm soát
            existingItem.UpdatedBy = updaterId;
            // EF Core sẽ tự động điền UpdatedDate nếu đã cấu hình SaveChangesAsync()

            await _context.SaveChangesAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> DeleteCatalogItemAsync(int id, int deleterId)
        {
            var itemToDelete = await _context.CourseCatalog.FindAsync(id);
            if (itemToDelete == null)
                return ServiceResult.Fail("Không tìm thấy khóa học để xóa.");

            // Thực hiện XÓA MỀM thay vì xóa cứng
            itemToDelete.Status = 0; // 0 = Inactive
            itemToDelete.UpdatedBy = deleterId;

            await _context.SaveChangesAsync();
            return ServiceResult.Success();
        }
        #endregion
    }
}