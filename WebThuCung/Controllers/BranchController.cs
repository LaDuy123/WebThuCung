using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;
using WebThuCung.Dto;
using WebThuCung.Models;

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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BranchDto branchDto)
        {
            // Kiểm tra xem chi nhánh có tồn tại không
            var existingBranch = _context.Branchs.FirstOrDefault(s => s.idBranch == branchDto.idBranch);
            if (existingBranch != null)
            {
                ModelState.AddModelError("", $"Branch with ID '{branchDto.idBranch}' already exists.");
                return View(branchDto); // Trả lại form với thông báo lỗi
            }

            // Tạo đối tượng Branch từ DTO
            var branch = new Branch
            {
                idBranch = branchDto.idBranch,
                nameBranch = branchDto.nameBranch,
                Logo = branchDto.Logo
            };

            // Thêm chi nhánh vào database
            _context.Branchs.Add(branch);
            _context.SaveChanges();

            // Chuyển hướng người dùng đến trang danh sách các chi nhánh sau khi tạo thành công
            return RedirectToAction("Index"); // Index là danh sách chi nhánh hoặc trang bạn muốn chuyển hướng đến
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var branch = _context.Branchs.FirstOrDefault(s => s.idBranch == id);
            if (branch == null)
            {
                return NotFound();
            }

            // Chuyển từ model sang DTO (nếu cần)
            var branchDto = new BranchDto
            {
                idBranch = branch.idBranch,
                nameBranch = branch.nameBranch,
                Logo = branch.Logo
            };

            return View(branchDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BranchDto branchDto)
        {
            if (ModelState.IsValid)
            {
                var branch = _context.Branchs.FirstOrDefault(s => s.idBranch == branchDto.idBranch);
                if (branch == null)
                {
                    return NotFound();
                }

                // Cập nhật các giá trị từ DTO vào model
                branch.nameBranch = branchDto.nameBranch;
                branch.Logo = branchDto.Logo;

                _context.SaveChanges();
                return RedirectToAction("Index"); // Quay lại trang danh sách Branch sau khi cập nhật
            }

            return View(branchDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var branch = _context.Branchs.Find(id);
            if (branch == null)
            {
                return NotFound();
            }

            _context.Branchs.Remove(branch);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Quay lại danh sách Branch sau khi xóa
        }


    }
}
