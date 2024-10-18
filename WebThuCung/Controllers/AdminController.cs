using Microsoft.AspNetCore.Mvc;

namespace WebThuCung.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
