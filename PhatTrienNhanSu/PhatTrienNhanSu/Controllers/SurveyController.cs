using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhatTrienNhanSu.DbContexts.Entities;
using PhatTrienNhanSu.Service.Executes.SurveyService;
using PhatTrienNhanSu.Service.Executes.SurveyService.Models;

namespace PhatTrienNhanSu.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ISurveyOne _one;
        private readonly ISurveyMany _many;
        private readonly ISurveyCommand _command;

        public SurveyController(ISurveyOne one, ISurveyMany many, ISurveyCommand command)
        {
            _one = one;
            _many = many;
            _command = command;
        }

        #region Chức năng Khảo sát cho Nhân viên

        // GET: /Survey/ hoặc /Survey/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Tạm thời hardcode EmployeeId. Trong thực tế, bạn sẽ lấy từ User.Claims
            var employeeId = 123;

            // Lấy kỳ khảo sát đang hoạt động
            var activePeriod = await _one.GetActiveSurveyPeriodAsync();
            if (activePeriod == null)
            {
                ViewBag.Message = "Hiện tại không có kỳ khảo sát nào đang diễn ra.";
                return View(); // Trả về View trống với thông báo
            }

            // Kiểm tra xem nhân viên đã nộp bài cho kỳ này chưa
            var hasSubmitted = await _one.HasEmployeeSubmittedSurveyAsync(employeeId, activePeriod.Id);
            if (hasSubmitted)
            {
                ViewBag.Message = "Bạn đã hoàn thành khảo sát cho kỳ này. Cảm ơn bạn!";
                return View(); // Trả về View trống với thông báo
            }

            // Nếu chưa nộp, chuẩn bị dữ liệu cho form
            var areaGroups = await _many.GetGroupedCatalogItemsAsync();
            var selections = await _many.GetInitialSelectionsAsync();

            var viewModel = new SurveyFormViewModel
            {
                SurveyPeriodId = activePeriod.Id,
                SurveyPeriodName = activePeriod.Name,
                AreaGroups = areaGroups,
                Selections = selections
            };

            return View(viewModel);
        }

        // POST: /Survey/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SurveyFormViewModel model)
        {
            var employeeId = 123; // Tạm thời hardcode
            var result = await _command.SubmitSurveyAsync(employeeId, model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Đã có lỗi xảy ra khi gửi khảo sát.";
                return RedirectToAction(nameof(Index)); // Quay lại form khảo sát để người dùng thử lại
            }

            TempData["SuccessMessage"] = "Cảm ơn bạn đã gửi khảo sát thành công!";
            return RedirectToAction("Index", "Home"); // Chuyển về trang chủ sau khi nộp thành công
        }

        #endregion

        #region Chức năng Quản lý Danh mục (CRUD) cho Admin

        // GET: /Survey/Catalog
        public async Task<IActionResult> Catalog()
        {
            var areaGroups = await _many.GetGroupedCatalogItemsAsync();
            return View(areaGroups);
        }

        // GET: /Survey/CreateCatalogItem
        public async Task<IActionResult> CreateCatalogItem()
        {
            await LoadProviderDropdown();
            return View();
        }

        // POST: /Survey/CreateCatalogItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCatalogItem(CourseCatalog model)
        {
            var creatorId = 1; // Tạm thời hardcode ID của admin

            // Bỏ qua validation cho các thuộc tính điều hướng mà form không gửi lên
            ModelState.Remove("Provider");
            ModelState.Remove("SurveyResponses");

            if (ModelState.IsValid)
            {
                var result = await _command.CreateCatalogItemAsync(model, creatorId);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Thêm mới khóa học vào danh mục thành công!";
                    return RedirectToAction(nameof(Catalog));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Không thể tạo khóa học.");
            }

            // Nếu model không hợp lệ, tải lại dropdown và hiển thị lại form
            await LoadProviderDropdown(model.ProviderID);
            return View(model);
        }

        // GET: /Survey/EditCatalogItem/5
        public async Task<IActionResult> EditCatalogItem(int id)
        {
            var item = await _one.GetCatalogItemByIdAsync(id);
            if (item == null) return NotFound();

            await LoadProviderDropdown(item.ProviderID);
            return View(item);
        }

        // POST: /Survey/EditCatalogItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCatalogItem(int id, CourseCatalog model)
        {
            if (id != model.Id) return BadRequest();

            var updaterId = 1; // Tạm thời hardcode
            ModelState.Remove("Provider");
            ModelState.Remove("SurveyResponses");

            if (ModelState.IsValid)
            {
                var result = await _command.UpdateCatalogItemAsync(model, updaterId);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Cập nhật khóa học thành công!";
                    return RedirectToAction(nameof(Catalog));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Không thể cập nhật khóa học.");
            }
            await LoadProviderDropdown(model.ProviderID);
            return View(model);
        }

        // POST: /Survey/DeleteCatalogItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCatalogItem(int id)
        {
            var deleterId = 1; // Tạm thời hardcode
            var result = await _command.DeleteCatalogItemAsync(id, deleterId);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Đã xóa (ẩn) khóa học thành công.";
            else
                TempData["ErrorMessage"] = result.ErrorMessage;

            return RedirectToAction(nameof(Catalog));
        }

        #endregion

        #region Hàm tiện ích

        // Hàm helper để tải danh sách Provider cho dropdown
        private async Task LoadProviderDropdown(object? selectedProvider = null)
        {
            // Tái sử dụng SurveyMany để lấy danh sách providers
            // Nếu bạn đã tạo ProviderMany, hãy inject và sử dụng nó ở đây để rõ ràng hơn
            var providers = await _many.GetActiveProvidersAsync();
            ViewBag.ProviderList = new SelectList(providers, "Id", "ProviderName", selectedProvider);
        }

        #endregion
    }
}