using Microsoft.AspNetCore.Mvc;

namespace PhatTrienNhanSu.Controllers
{
    public class TrainingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
