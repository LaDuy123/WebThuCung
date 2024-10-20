using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;

namespace WebThuCung.Controllers
{
    public class OrderController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public OrderController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var orders = _context.Orders.ToList();

            // Truyền danh sách admin sang view
            return View(orders);
        }
    }
}
