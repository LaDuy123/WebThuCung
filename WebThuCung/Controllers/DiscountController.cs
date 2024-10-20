using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;

namespace WebThuCung.Controllers
{
    public class DiscountController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public DiscountController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var discounts = _context.Discounts.ToList();

            // Truyền danh sách admin sang view
            return View(discounts);
        }
    }
}
