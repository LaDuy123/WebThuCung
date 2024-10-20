using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;

namespace WebThuCung.Controllers
{
    public class CustomerController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public CustomerController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var customers = _context.Customers.ToList();

            // Truyền danh sách admin sang view
            return View(customers);
        }
    }
}
