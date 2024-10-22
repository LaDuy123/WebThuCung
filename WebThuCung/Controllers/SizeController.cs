using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebThuCung.Data;
using WebThuCung.Dto;
using WebThuCung.Models;

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

            return View(sizes);
        }
        public IActionResult Create()
        {
            var products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct,
                Text = p.nameProduct
            }).ToList();

            // Truyền danh sách sản phẩm vào ViewBag để sử dụng trong view
            ViewBag.Products = products;
            return View();
        }
        [HttpPost]
        public IActionResult Create(SizeDto sizeDto)
        {
            var products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct,
                Text = p.nameProduct
            }).ToList();

            // Truyền danh sách sản phẩm vào ViewBag để sử dụng trong view
            ViewBag.Products = products;
            if (ModelState.IsValid)
            {
              
                var existingSize = _context.Sizes.FirstOrDefault(s => s.idSize == sizeDto.idSize);
                if (existingSize != null)
                {
                    ModelState.AddModelError("", $"Size with ID '{sizeDto.idSize}' already exists.");
                    return View(sizeDto); // Trả lại form với thông báo lỗi
                }
                            
                var size = new Size
                {
                    idSize = sizeDto.idSize,
                    nameSize = sizeDto.nameSize,
                    idProduct = sizeDto.idProduct
                };
                _context.Sizes.Add(size);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
         
            return View(sizeDto);
        }

        // Hiển thị form Edit
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Tìm Size dựa trên id, sử dụng FirstOrDefault để kiểm tra an toàn hơn
            var size = _context.Sizes.FirstOrDefault(s => s.idSize == id);
            if (size == null)
            {
                return NotFound();
            }
          
            // Lấy danh sách sản phẩm để hiển thị trong dropdown
            var products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct,
                Text = p.nameProduct
            }).ToList();

            // Truyền danh sách sản phẩm vào ViewBag để sử dụng trong view
            ViewBag.Products = products;

            // Chuyển Size entity sang SizeDto để sử dụng trong view
            var sizeDto = new SizeDto
            {
                idSize = size.idSize,
                nameSize = size.nameSize,
                idProduct = size.idProduct
            };

            return View(sizeDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SizeDto sizeDto)
        {
            if (ModelState.IsValid)
            {
                var size = _context.Sizes.FirstOrDefault(s => s.idSize == sizeDto.idSize);
                if (size == null)
                {
                    return NotFound();
                }
                
                // Cập nhật các giá trị từ DTO vào model
                size.nameSize = sizeDto.nameSize;
                size.idProduct = sizeDto.idProduct;

             
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
              

                return RedirectToAction("Index"); // Quay lại trang danh sách Size sau khi cập nhật
            }

            // Nếu ModelState không hợp lệ, truyền lại danh sách sản phẩm vào ViewBag
            var products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct,
                Text = p.nameProduct
            }).ToList();
            ViewBag.Products = products;

            return View(sizeDto); // Trả về form với thông báo lỗi (nếu có)
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var size = _context.Sizes.Find(id);
            if (size == null)
            {
                return NotFound();
            }

            _context.Sizes.Remove(size);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Quay lại danh sách Size sau khi xóa
        }

    }
}
