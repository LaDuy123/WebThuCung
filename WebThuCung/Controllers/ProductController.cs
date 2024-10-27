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
                            .ToList();

            // Truyền danh sách admin sang viewsds
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

            ViewBag.Pets = _context.Pets.Select(p => new SelectListItem
            {
                Value = p.idPet,
                Text = p.namePet
            }).ToList();

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public IActionResult Create(ProductCreateDto productDto)
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
                    ModelState.AddModelError("idProduct", $"Product with ID '{productDto.idProduct}' already exists.");
                    return View(productDto); // Trả lại form với thông báo lỗi
                }
                string ImageFilePath = null;
                if (productDto.Image != null && productDto.Image.Length > 0)
                {
                    var ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", productDto.Image.FileName);
                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                       productDto.Image.CopyTo(stream);
                    }
                    ImageFilePath = productDto.Image.FileName; // Cập nhật tên tệp Image
                }
                var product = new Product
                {
                    idProduct = productDto.idProduct,
                    nameProduct = productDto.nameProduct,
                    sellPrice = productDto.sellPrice,
                    idBranch = productDto.idBranch,
                    idCategory = productDto.idCategory,
                    idPet = productDto.idPet,
                    Quantity = productDto.Quantity,
                    Image = ImageFilePath,
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
                idPet = product.idPet,
                Quantity = product.Quantity,
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
                product.idPet = productDto.idPet;
                product.Quantity = productDto.Quantity;
                product.Description = productDto.Description;
                if (productDto.Image != null && productDto.Image.Length > 0)
                {
                    var cvPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", productDto.Image.FileName);
                    if (System.IO.File.Exists(cvPath))
                    {
                        System.IO.File.Delete(cvPath);
                    }

                    using (var stream = new FileStream(cvPath, FileMode.Create, FileAccess.Write))
                    {
                        productDto.Image.CopyTo(stream);
                    }
                    product.Image = productDto.Image.FileName;
                }

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

        [HttpGet]
        public IActionResult Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); // Trả về lỗi 404 nếu idProduct không tồn tại
            }
            var customerid = GetCustomerIdFromSession();
            // Tìm sản phẩm dựa trên idProduct
            var product = _context.Products
                .Include(sp => sp.Branch)           // Baogồm thông tin thương hiệu (Branch)
                .Include(sp => sp.Category)     
                .Include(sp => sp.Pet)         // Bao gồm thông tin loại (Category)
                                                    // Bao gồm thông tin loại (Category)
                .Include(sp => sp.ImageProducts)    // Bao gồm thông tin các hình ảnh sản phẩm
              .Include(sp => sp.ProductColors)      // Bao gồm ProductColors
        .ThenInclude(pc => pc.Color)      // Bao gồm thông tin Color
    .Include(sp => sp.ProductSizes)       // Bao gồm ProductSizes
        .ThenInclude(ps => ps.Size)         // Bao gồm thông tin kích thước
                .Include(sp => sp.Discounts)        // Bao gồm thông tin giảm giá
                .Where(p => p.idProduct == id)
                .Select(p => new ProductViewDto
                {
                    idProduct = p.idProduct,
                    nameProduct = p.nameProduct,
                    sellPrice = p.sellPrice,
                    Image = p.Image, // Hình ảnh chính của sản phẩm
                    idBranch = p.idBranch,
                    idCategori = p.idCategory,
                    idPet = p.idPet,
                    nameBranch = p.Branch != null ? p.Branch.nameBranch : "N/A", // Tên thương hiệu, nếu có
                    namePet = p.Pet != null ? p.Pet.namePet : "N/A", // Tên thương hiệu, nếu có
                    nameCategory = p.Category != null ? p.Category.nameCategory : "N/A", // Tên loại, nếu có
                    Quantity = p.Quantity,
                    Description = p.Description,
                    Colors = p.ProductColors != null ? p.ProductColors.Select(pc => pc.Color.nameColor).ToList() : new List<string>(), // Lấy danh sách màu sắc
                    Sizes = p.ProductSizes != null ? p.ProductSizes.Select(ps => ps.Size.nameSize).ToList() : new List<string>(),      // Lấy danh sách kích thước
                    Logo = p.Branch != null ? p.Branch.Logo : null, // Logo của chi nhánh
                    Discounts = p.Discounts != null ? p.Discounts.Select(s => s.discountPercent).ToList() : new List<int>(), // Phần trăm giảm giá
                    ImageProducts = p.ImageProducts != null ? p.ImageProducts.Select(i => i.Image).ToList() : new List<string>() // Hình ảnh sản phẩm khác
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound(); // Nếu không tìm thấy sản phẩm
            }

            // Trả về view với thông tin chi tiết của sản phẩm
            return View(product);
        }
        private int GetCustomerIdFromSession()
        {
            var customerEmail = HttpContext.Session.GetString("email");
            var customer = _context.Customers.FirstOrDefault(c => c.Email == customerEmail);
            return customer?.idCustomer ?? 0;
        }
        private string GenerateOrderId()
        {
            Random random = new Random();
            int number = random.Next(10000, 99999); // Tạo ra một số ngẫu nhiên từ 10000 đến 99999
            return "O" + number.ToString(); // Kết hợp với "O" để tạo ra idOrder
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddToCart([FromBody] AddToCartViewDto model)
        {
            try
            {
                if (model == null || !ModelState.IsValid) // Kiểm tra nếu model là null hoặc không hợp lệ
                {
                    return Json(new { success = false, message = "Invalid data." });
                }

                int idCustomer = GetCustomerIdFromSession();

                // Kiểm tra nếu idCustomer bằng 0, chuyển đến trang đăng nhập
                if (idCustomer == 0)
                {
                    return Json(new { success = false, redirectUrl = Url.Action("Login", "User") });
                }

                // Kiểm tra đơn hàng hiện tại
                var order = _context.Orders.FirstOrDefault(o => o.idCustomer == idCustomer && o.statusOrder == OrderStatus.Pending);

                // Nếu không có đơn hàng hiện tại, tạo đơn hàng mới
                if (order == null)
                {
                    order = new Order
                    {
                        idOrder = GenerateOrderId(), // Gán idOrder với hàm GenerateOrderId
                        idCustomer = idCustomer,
                        statusOrder = OrderStatus.Pending
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }

                // Kiểm tra xem sản phẩm đã có trong đơn hàng chưa với cùng idProduct
                var existingOrderDetail = _context.DetailOrders.FirstOrDefault(od =>
                    od.idOrder == order.idOrder &&
                    od.idProduct == model.ProductId &&
                    od.nameColor == model.Color &&
                    od.nameSize == model.Size);

                if (existingOrderDetail != null)
                {
                    // Nếu sản phẩm đã tồn tại với màu sắc và kích thước giống nhau, cập nhật số lượng
                    existingOrderDetail.Quantity += model.Quantity;
                    existingOrderDetail.totalPrice = existingOrderDetail.Quantity * _context.Products.First(p => p.idProduct == model.ProductId).sellPrice; // Cập nhật tổng giá
                    _context.DetailOrders.Update(existingOrderDetail);
                }
                else
                {
                    // Kiểm tra xem sản phẩm đã có trong đơn hàng cũ chưa với cùng idProduct
                    var sameProductDetail = _context.DetailOrders.FirstOrDefault(od =>
                        od.idOrder == order.idOrder &&
                        od.idProduct == model.ProductId);

                    if (sameProductDetail != null)
                    {
                        // Nếu trùng idProduct nhưng khác màu hoặc kích thước, tạo đơn hàng mới
                        var newOrder = new Order
                        {
                            idOrder = GenerateOrderId(), // Tạo mã đơn hàng mới
                            idCustomer = idCustomer,
                            statusOrder = OrderStatus.Pending
                        };
                        _context.Orders.Add(newOrder);
                        _context.SaveChanges();

                        // Thêm chi tiết đơn hàng mới
                        var newOrderDetail = new DetailOrder
                        {
                            idOrder = newOrder.idOrder,
                            idProduct = model.ProductId,
                            nameColor = model.Color,
                            nameSize = model.Size,
                            Quantity = model.Quantity,
                            totalPrice = model.Quantity * _context.Products.First(p => p.idProduct == model.ProductId).sellPrice // Tính tổng giá
                        };

                        _context.DetailOrders.Add(newOrderDetail);
                    }
                    else
                    {
                        // Nếu sản phẩm hoàn toàn mới (không tồn tại trong đơn hàng hiện tại), tạo chi tiết đơn hàng mới
                        var newOrderDetail = new DetailOrder
                        {
                            idOrder = order.idOrder,
                            idProduct = model.ProductId,
                            nameColor = model.Color,
                            nameSize = model.Size,
                            Quantity = model.Quantity,
                            totalPrice = model.Quantity * _context.Products.First(p => p.idProduct == model.ProductId).sellPrice // Tính tổng giá
                        };

                        _context.DetailOrders.Add(newOrderDetail);
                    }
                }

                _context.SaveChanges(); // Lưu các thay đổi vào database

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "No inner exception.";
                return Json(new { success = false, message = ex.Message + " Inner Exception: " + innerException });
            }
        }

        public IActionResult SavedProduct()
        {
            // Lấy customerId từ session (giả sử bạn đã lưu customerId trong session khi người dùng đăng nhập)
            var customerId = GetCustomerIdFromSession();

            // Kiểm tra xem customerId có hợp lệ không
            if (customerId == 0)
            {
                // Nếu không có customerId trong session, chuyển hướng đến trang đăng nhập hoặc xử lý lỗi
                return RedirectToAction("Login", "User");
            }

            // Lấy danh sách công việc đã lưu cho customer cụ thể
            var saveProduct = _context.SaveProducts
                .Where(s => s.idCustomer == customerId)  // Lọc theo customerId
                .Include(s => s.Product)
                .ToList();

            // Nếu không có công việc nào được lưu
            if (!saveProduct.Any())
            {
                // Có thể trả về một view khác hoặc hiển thị thông báo không có công việc đã lưu
                ViewBag.Message = "Bạn chưa lưu công việc nào.";
                return View(new List<SaveProduct>());
            }

            // Trả về view với danh sách công việc đã lưu
            return View(saveProduct);
        }




        [HttpPost]
        public async Task<IActionResult> SaveProduct(string idproduct)
        {
            try
            {
                int customerid = GetCustomerIdFromSession();

                if (customerid == 0)
                {
                    return Json(new { success = false, message = "Please log in to save products." });
                }

                var existingSaveProduct = await _context.SaveProducts
                    .FirstOrDefaultAsync(s => s.idProduct == idproduct && s.idCustomer == customerid);

                if (existingSaveProduct != null)
                {
                    _context.SaveProducts.Remove(existingSaveProduct);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Product has been removed from saved products." });
                }
                else
                {
                    var saveproduct = new SaveProduct
                    {
                        idProduct = idproduct,
                        idCustomer = customerid,
                        SavedAt = DateTime.Now
                    };

                    _context.SaveProducts.Add(saveproduct);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Product has been saved successfully!" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving product: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while saving the product." });
            }
        }

        [HttpPost]
        public IActionResult DeleteSaveProduct(string productid)
        {
            var customerId = GetCustomerIdFromSession(); // Giả sử có hàm để lấy customer hiện tại

            if (customerId == null)
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            var saveProduct = _context.SaveProducts
                .FirstOrDefault(sj => sj.idProduct == productid && sj.idCustomer == customerId);

            if (saveProduct != null)
            {
                _context.SaveProducts.Remove(saveProduct);
                _context.SaveChanges();
                return Json(new { success = true, message = "Job has been deleted successfully!" });
            }

            return Json(new { success = false, message = "Saved job not found!" });
        }





    }
}
