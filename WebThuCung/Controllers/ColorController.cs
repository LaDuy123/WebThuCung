using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;
using WebThuCung.Dto;
using WebThuCung.Models;

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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ColorDto colorDto)
        {
            if (ModelState.IsValid)
            {
                var existingColor = _context.Colors.FirstOrDefault(s => s.idColor == colorDto.idColor);
                if (existingColor != null)
                {
                    ModelState.AddModelError("", $"Color with ID '{colorDto.idColor}' already exists.");
                    return View(colorDto); // Trả lại form với thông báo lỗi
                }
                var color = new Color
                {
                    idColor = colorDto.idColor,
                    nameColor = colorDto.nameColor
                };
                _context.Colors.Add(color);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(colorDto);
        }

        // Hiển thị form Edit
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var color = _context.Colors.FirstOrDefault(s => s.idColor == id);
            if (color == null)
            {
                return NotFound();
            }

            // Chuyển từ model sang DTO (nếu cần)
            var colorDto = new ColorDto
            {
                idColor = color.idColor,
                nameColor = color.nameColor,

            };

            return View(colorDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ColorDto colorDto)
        {
            if (ModelState.IsValid)
            {
                var color = _context.Colors.FirstOrDefault(s => s.idColor == colorDto.idColor);
                if (color == null)
                {
                    return NotFound();
                }

                // Cập nhật các giá trị từ DTO vào model
                color.nameColor = colorDto.nameColor;

                _context.SaveChanges();
                return RedirectToAction("Index"); // Quay lại trang danh sách Color sau khi cập nhật
            }

            return View(colorDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var color = _context.Colors.Find(id);
            if (color == null)
            {
                return NotFound();
            }

            _context.Colors.Remove(color);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Quay lại danh sách Color sau khi xóa
        }

    }
}
