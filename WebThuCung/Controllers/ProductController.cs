using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebThuCung.Data;
using WebThuCung.Dto;
using WebThuCung.Models;

namespace WebThuCung.Controllers
{
    public class ProductController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public ProductController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var products = _context.Products
                            .Include(sp => sp.Branch)  // Bao gồm thông tin thương hiệu
                            .Include(sp => sp.Category)        // Bao gồm thông tin loại
                            .Include(sp => sp.Color).ToList();

            // Truyền danh sách admin sang view
            return View(products);
        }
        public IActionResult Create()
        {
            // Có thể truyền danh sách chi nhánh, danh mục, màu sắc, v.v. vào ViewBag nếu cần
            ViewBag.Branchs = _context.Branchs.Select(b => new SelectListItem
            {
                Value = b.idBranch,
                Text = b.nameBranch
            }).ToList();

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.idCategory,
                Text = c.nameCategory
            }).ToList();

            ViewBag.Colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.idColor,
                Text = c.nameColor
            }).ToList();

            ViewBag.Pets = _context.Pets.Select(p => new SelectListItem
            {
                Value = p.idPet,
                Text = p.namePet
            }).ToList();

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            ViewBag.Branchs = _context.Branchs.Select(b => new SelectListItem
            {
                Value = b.idBranch,
                Text = b.nameBranch
            }).ToList();

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.idCategory,
                Text = c.nameCategory
            }).ToList();

            ViewBag.Colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.idColor,
                Text = c.nameColor
            }).ToList();

            ViewBag.Pets = _context.Pets.Select(p => new SelectListItem
            {
                Value = p.idPet,
                Text = p.namePet
            }).ToList();
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.idProduct == productDto.idProduct);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("", $"Product with ID '{productDto.idProduct}' already exists.");
                    return View(productDto); // Trả lại form với thông báo lỗi
                }

                var product = new Product
                {
                    idProduct = productDto.idProduct,
                    nameProduct = productDto.nameProduct,
                    sellPrice = productDto.sellPrice,
                    idBranch = productDto.idBranch,
                    idCategory = productDto.idCategory,
                    idColor = productDto.idColor,
                    idPet = productDto.idPet,
                    Quantity = productDto.Quantity,
                    Image = productDto.Image,
                    Description = productDto.Description
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Index"); // Chuyển hướng đến trang danh sách sản phẩm
            }

            return View(productDto);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(p => p.idProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                idProduct = product.idProduct,
                nameProduct = product.nameProduct,
                sellPrice = product.sellPrice,
                idBranch = product.idBranch,
                idCategory = product.idCategory,
                idColor = product.idColor,
                idPet = product.idPet,
                Quantity = product.Quantity,
                Image = product.Image,
                Description = product.Description
            };

            // Truyền danh sách chi nhánh, danh mục, màu sắc vào ViewBag
            ViewBag.Branchs = _context.Branchs.Select(b => new SelectListItem
            {
                Value = b.idBranch,
                Text = b.nameBranch
            }).ToList();

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.idCategory,
                Text = c.nameCategory
            }).ToList();

            ViewBag.Colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.idColor,
                Text = c.nameColor
            }).ToList();

            ViewBag.Pets = _context.Pets.Select(p => new SelectListItem
            {
                Value = p.idPet,
                Text = p.namePet
            }).ToList();

            return View(productDto);
        }

        // POST: Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductDto productDto)
        {
            ViewBag.Branchs = _context.Branchs.Select(b => new SelectListItem
            {
                Value = b.idBranch,
                Text = b.nameBranch
            }).ToList();

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.idCategory,
                Text = c.nameCategory
            }).ToList();

            ViewBag.Colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.idColor,
                Text = c.nameColor
            }).ToList();

            ViewBag.Pets = _context.Pets.Select(p => new SelectListItem
            {
                Value = p.idPet,
                Text = p.namePet
            }).ToList();
            if (ModelState.IsValid)
            {
                var product = _context.Products.FirstOrDefault(p => p.idProduct == productDto.idProduct);
                if (product == null)
                {
                    return NotFound();
                }

                // Cập nhật các giá trị từ DTO vào model
                product.nameProduct = productDto.nameProduct;
                product.sellPrice = productDto.sellPrice;
                product.idBranch = productDto.idBranch;
                product.idCategory = productDto.idCategory;
                product.idColor = productDto.idColor;
                product.idPet = productDto.idPet;
                product.Quantity = productDto.Quantity;
                product.Image = productDto.Image;
                product.Description = productDto.Description;

                _context.SaveChanges();
                return RedirectToAction("Index"); // Quay lại trang danh sách sản phẩm sau khi cập nhật
            }

            return View(productDto);
        }

        // POST: Product/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Quay lại danh sách sản phẩm sau khi xóa
        }


    }
}
