using Microsoft.AspNetCore.Mvc;

namespace PhatTrienNhanSu.Controllers
{

    public class SurveyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}
