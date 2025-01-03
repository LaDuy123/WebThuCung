﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebThuCung.Data;
using WebThuCung.Dto;
using WebThuCung.Models;

namespace WebThuCung.Controllers
{
    public class ImageProductController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public ImageProductController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var images = _context.ImageProducts.ToList();

            // Truyền danh sách admin sang view
            return View(images);
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
        public IActionResult Create(ImageProductCreateDto imageProductDto)
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

                var existingImageProduct = _context.ImageProducts.FirstOrDefault(s => s.idImageProduct == imageProductDto.idImageProduct);
                if (existingImageProduct != null)
                {
                    ModelState.AddModelError("idImageProduct", $"ImageProduct with ID '{imageProductDto.idImageProduct}' already exists.");
                    return View(imageProductDto); // Trả lại form với thông báo lỗi
                }

                string ImageFilePath = null;
                if (imageProductDto.Image != null && imageProductDto.Image.Length > 0)
                {
                    var ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageProductDto.Image.FileName);
                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                        imageProductDto.Image.CopyTo(stream);
                    }
                    ImageFilePath = imageProductDto.Image.FileName; // Cập nhật tên tệp Image
                }

                var imageProduct = new ImageProduct
                {
                    idImageProduct = imageProductDto.idImageProduct,
                    Image = ImageFilePath,
                    idProduct = imageProductDto.idProduct
                };
                _context.ImageProducts.Add(imageProduct);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(imageProductDto);
        }

        // Hiển thị form Edit
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Tìm ImageProduct dựa trên id, sử dụng FirstOrDefault để kiểm tra an toàn hơn
            var imageProduct = _context.ImageProducts.FirstOrDefault(s => s.idImageProduct == id);
            if (imageProduct == null)
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

            // Chuyển ImageProduct entity sang ImageProductDto để sử dụng trong view
            var imageProductDto = new ImageProductDto
            {
                idImageProduct = imageProduct.idImageProduct,
                idProduct = imageProduct.idProduct
            };

            return View(imageProductDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ImageProductDto imageProductDto)
        {
            if (ModelState.IsValid)
            {
                var imageProduct = _context.ImageProducts.FirstOrDefault(s => s.idImageProduct == imageProductDto.idImageProduct);
                if (imageProduct == null)
                {
                    return NotFound();
                }

         
                imageProduct.idProduct = imageProductDto.idProduct;
                if (imageProductDto.Image != null && imageProductDto.Image.Length > 0)
                {
                    var cvPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageProductDto.Image.FileName);
                    if (System.IO.File.Exists(cvPath))
                    {
                        System.IO.File.Delete(cvPath);
                    }

                    using (var stream = new FileStream(cvPath, FileMode.Create, FileAccess.Write))
                    {
                        imageProductDto.Image.CopyTo(stream);
                    }
                    imageProduct.Image = imageProductDto.Image.FileName;
                }            
                 

                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu


                return RedirectToAction("Index"); // Quay lại trang danh sách ImageProduct sau khi cập nhật
            }

            // Nếu ModelState không hợp lệ, truyền lại danh sách sản phẩm vào ViewBag
            var products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct,
                Text = p.nameProduct
            }).ToList();
            ViewBag.Products = products;

            return View(imageProductDto); // Trả về form với thông báo lỗi (nếu có)
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var imageProduct = _context.ImageProducts.Find(id);
            if (imageProduct == null)
            {
                return NotFound();
            }

            _context.ImageProducts.Remove(imageProduct);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Quay lại danh sách ImageProduct sau khi xóa
        }

    }
}
