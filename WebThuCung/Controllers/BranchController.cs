using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;

namespace WebThuCung.Controllers
{
    public class BranchController : Controller
    {

        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public BranchController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var branchs = _context.Branchs.ToList();

            // Truyền danh sách admin sang view
            return View(branchs);
        }
    }
}
