using Microsoft.AspNetCore.Mvc;
using PhatTrienNhanSu.DbContexts.Entities;
using PhatTrienNhanSu.Service.Executes.ProviderService;

namespace PhatTrienNhanSu.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IProviderMany _many;
        private readonly IProviderOne _one;
        private readonly IProviderCommand _command;

        public ProviderController(IProviderMany many, IProviderOne one, IProviderCommand command)
        {
            _many = many;
            _one = one;
            _command = command;
        }

        // GET: /Provider
        public async Task<IActionResult> Index()
        {
            var providers = await _many.GetAllProvidersAsync();
            return View(providers);
        }

        // GET: /Provider/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Provider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingProvider provider)
        {
            var creatorId = 1; // Tạm thời hardcode
            ModelState.Remove("CourseCatalogs"); // Bỏ qua validation cho thuộc tính điều hướng
            if (ModelState.IsValid)
            {
                var result = await _command.CreateProviderAsync(provider, creatorId);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Thêm mới nhà cung cấp thành công!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Không thể tạo nhà cung cấp.");
            }
            return View(provider);
        }

        // GET: /Provider/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var provider = await _one.GetProviderByIdAsync(id);
            if (provider == null) return NotFound();
            return View(provider);
        }

        // POST: /Provider/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingProvider provider)
        {
            if (id != provider.Id) return BadRequest();

            var updaterId = 1; // Tạm thời hardcode
            ModelState.Remove("CourseCatalogs");
            if (ModelState.IsValid)
            {
                var result = await _command.UpdateProviderAsync(provider, updaterId);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Cập nhật nhà cung cấp thành công!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Không thể cập nhật nhà cung cấp.");
            }
            return View(provider);
        }

        // POST: /Provider/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleterId = -1; // Tạm thời hardcode
            var result = await _command.DeleteProviderAsync(id, deleterId);
            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Đã xóa (ẩn) nhà cung cấp thành công.";
            else
                TempData["ErrorMessage"] = result.ErrorMessage;

            return RedirectToAction(nameof(Index));
        }
    }
}