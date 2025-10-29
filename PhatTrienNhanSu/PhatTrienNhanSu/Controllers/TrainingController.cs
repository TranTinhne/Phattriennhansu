using Microsoft.AspNetCore.Mvc;
using PhatTrienNhanSu.Service.TrainingService;
using PhatTrienNhanSu.Service.TrainingService.Models;

namespace PhatTrienNhanSu.Controllers
{
    // Trong thực tế, bạn sẽ thêm [Authorize] ở đây sau khi có hệ thống đăng nhập
    // [Authorize] 
    public class TrainingController : Controller
    {
        private readonly ITrainingOne _oneService;
        private readonly ITrainingMany _manyService;
        private readonly ITrainingCommand _commandService;

        public TrainingController(
            ITrainingOne oneService,
            ITrainingMany manyService,
            ITrainingCommand commandService)
        {
            _oneService = oneService;
            _manyService = manyService;
            _commandService = commandService;
        }

        /// <summary>
        /// Action để hiển thị danh sách các chương trình đào tạo đang hoạt động.
        /// </summary>
        // GET: /Training/ or /Training/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var programs = await _manyService.GetActiveProgramsAsync();
            // Truyền danh sách chương trình vào View để hiển thị
            return View(programs);
        }

        /// <summary>
        /// Action để hiển thị trang chi tiết của một chương trình, bao gồm các khóa học.
        /// </summary>
        // GET: /Training/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var programDetail = await _oneService.GetProgramDetailsByIdAsync(id);
            if (programDetail == null)
            {
                return NotFound(); // Không tìm thấy chương trình
            }
            return View(programDetail);
        }

        /// <summary>
        /// Action xử lý khi người dùng nhấn nút "Đăng ký".
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // Chống tấn công CSRF
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu gửi lên không hợp lệ, quay lại trang chi tiết
                TempData["ErrorMessage"] = "Dữ liệu đăng ký không hợp lệ.";
                return RedirectToAction("Details", new { id = request.ProgramId });
            }

            // Trong thực tế, EmployeeId sẽ được lấy từ User.Claims sau khi đăng nhập.
            // Tạm thời hardcode để kiểm thử.
            var employeeId = 123;

            var result = await _commandService.RegisterForCourseAsync(employeeId, request.CourseId);

            if (!result.IsSuccess)
            {
                // Nếu Service trả về lỗi, hiển thị thông báo cho người dùng
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction("Details", new { id = request.ProgramId });
            }

            // Nếu thành công, chuyển hướng đến trang lịch sử đăng ký
            TempData["SuccessMessage"] = "Bạn đã đăng ký khóa học thành công!";
            return RedirectToAction("MyRegistrations");
        }

        /// <summary>
        /// Action để hiển thị các khóa học mà người dùng hiện tại đã đăng ký.
        /// </summary>
        // GET: /Training/MyRegistrations
        [HttpGet]
        public async Task<IActionResult> MyRegistrations()
        {
            var employeeId = 123; // Tạm thời hardcode
            var registrations = await _manyService.GetMyRegistrationsAsync(employeeId);
            return View(registrations);
        }
    }
}