using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;

namespace WebThuCung.Controllers
{
    public class SizeController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public SizeController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var sizes = _context.Sizes.ToList();

            // Truyền danh sách admin sang view
            return View(sizes);
        }
    }
}
