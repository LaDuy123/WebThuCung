using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;

namespace WebThuCung.Controllers
{
    public class ColorController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public ColorController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var colors = _context.Colors.ToList();

            // Truyền danh sách admin sang view
            return View(colors);
        }
    }
}
