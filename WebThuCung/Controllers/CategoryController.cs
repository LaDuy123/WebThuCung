using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebThuCung.Data;

namespace WebThuCung.Controllers
{
    public class CategoryController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public CategoryController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var categories = _context.Categories.ToList();

            // Truyền danh sách admin sang view
            return View(categories);
        }
    
    }
}
