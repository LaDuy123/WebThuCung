﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using WebThuCung.Data;
using WebThuCung.Dto;

using WebThuCung.Models;

namespace WebThuCung.Controllers
{
    public class ProductController : BaseController
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public ProductController(PetContext context) : base(context)
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
        [HttpGet]
        public IActionResult SearchByPet(string petName)
        {
            var customerId = GetCustomerIdFromSession();
            // Lấy danh sách các sản phẩm theo tên thú cưng (namePet) và ánh xạ sang ProductViewDto
            var products = _context.Products
                                   .Include(p => p.Pet)
                                   .Include(p => p.Branch)
                                   .Include(p => p.Category)
                                   .Where(p => p.Pet.namePet == petName) // Tìm kiếm theo tên thú cưng
                                   .Select(sp => new ProductViewDto
                                   {
                                       idProduct = sp.idProduct,
                                       nameProduct = sp.nameProduct,
                                       sellPrice = sp.sellPrice,
                                       Image = sp.Image,
                                       idBranch = sp.idBranch,
                                       idCategori = sp.idCategory,
                                       nameBranch = sp.Branch.nameBranch,       // Lấy tên thương hiệu từ đối tượng liên quan
                                       nameCategory = sp.Category.nameCategory, // Lấy tên loại từ đối tượng liên quan
                                       Quantity = sp.Quantity,
                                       Description = sp.Description,
                                       Logo = sp.Branch.Logo                    // Lấy logo từ đối tượng liên quan
                                   })
                                   .ToList();
            var savedProductIds = _context.SaveProducts
            .Where(s => s.idCustomer == customerId)
            .Select(s => s.idProduct)
            .ToList();

            // Gán danh sách cho ViewBag để sử dụng trong View
            ViewBag.SavedProductIds = savedProductIds;
            // Truyền dữ liệu sản phẩm vào View
            return View(products);
        }
        [HttpGet]
        public IActionResult Shop()
        {
            var customerId = GetCustomerIdFromSession();

            // Bắt đầu truy vấn
            var productsQuery = _context.Products
                .Include(sp => sp.Branch)  // Bao gồm thông tin thương hiệu
                .Include(sp => sp.Category) // Bao gồm thông tin loại
                .AsQueryable(); // Chuyển đổi thành IQueryable để dễ dàng thêm điều kiện


            // Chọn các thuộc tính cần thiết
            var allProducts = productsQuery
                .Select(sp => new ProductViewDto
                {
                    idProduct = sp.idProduct,
                    nameProduct = sp.nameProduct,
                    sellPrice = sp.sellPrice,
                    Image = sp.Image,
                    idBranch = sp.idBranch,
                    idCategori = sp.idCategory,
                    nameBranch = sp.Branch.nameBranch, // Lấy tên thương hiệu từ đối tượng liên quan
                    nameCategory = sp.Category.nameCategory, // Lấy tên loại từ đối tượng liên quan
                    Quantity = sp.Quantity,
                    Description = sp.Description, // Lấy tên màu sắc từ đối tượng liên quan
                    Logo = sp.Branch.Logo // Lấy logo từ đối tượng liên quan
                })
                .OrderBy(p => p.idProduct)
                .Take(9) // Lấy ra 6 sản phẩm
                .ToList();

            // Lấy danh sách ID sản phẩm đã lưu
            var savedProductIds = _context.SaveProducts
                .Where(s => s.idCustomer == customerId)
                .Select(s => s.idProduct)
                .ToList();

            // Gán danh sách cho ViewBag để sử dụng trong View
            ViewBag.SavedProductIds = savedProductIds;



            return View(allProducts);
        }
        [HttpGet]
        public IActionResult ProductByCategory(string idCategory)
        {
            var customerId = GetCustomerIdFromSession();

            // Kiểm tra nếu idCategory là "Shop by Category" hoặc rỗng thì lấy toàn bộ sản phẩm
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Branch)
                .AsQueryable();

            if (!string.IsNullOrEmpty(idCategory) && idCategory != "Shop by Category")
            {
                productsQuery = productsQuery.Where(p => p.idCategory == idCategory);
            }

            // Lấy dữ liệu cần thiết và chuyển sang DTO
            var products = productsQuery
                .Select(p => new ProductViewDto
                {
                    idProduct = p.idProduct,
                    nameProduct = p.nameProduct,
                    sellPrice = p.sellPrice,
                    Image = p.Image,
                    idBranch = p.idBranch,
                    idCategori = p.idCategory,
                    nameBranch = p.Branch.nameBranch,
                    nameCategory = p.Category.nameCategory,
                    Quantity = p.Quantity,
                    Description = p.Description,
                    Logo = p.Branch.Logo
                })
                .ToList();

            // Lấy danh sách sản phẩm đã lưu của khách hàng hiện tại
            var savedProductIds = _context.SaveProducts
                .Where(s => s.idCustomer == customerId)
                .Select(s => s.idProduct)
                .ToList();

            // Gán danh sách ID sản phẩm đã lưu vào ViewBag để sử dụng trong View
            ViewBag.SavedProductIds = savedProductIds;

            // Trả về Partial View với danh sách sản phẩm
            return PartialView("_ProductByCategory", products);
        }


        [HttpGet]
        public IActionResult FilterProducts(string categoryId, string branchId, string petId)
        {
            var customerId = GetCustomerIdFromSession();

            // Khởi tạo truy vấn với thông tin liên kết cần thiết
            var productsQuery = _context.Products
                .Include(sp => sp.Branch)
                .Include(sp => sp.Category)
                .Include(sp => sp.Pet) // Bao gồm thông tin thú cưng
                .AsQueryable(); // Chuyển đổi thành IQueryable để có thể áp dụng các điều kiện

            if (!string.IsNullOrEmpty(categoryId))
            {
                productsQuery = productsQuery.Where(p => p.idCategory == categoryId);
            }

            if (!string.IsNullOrEmpty(branchId))
            {
                productsQuery = productsQuery.Where(p => p.idBranch == branchId);
            }

            if (!string.IsNullOrEmpty(petId))
            {
                productsQuery = productsQuery.Where(p => p.idPet == petId);
            }

            // Lấy danh sách sản phẩm từ truy vấn
            var products = productsQuery
                .Select(sp => new ProductViewDto
                {
                    idProduct = sp.idProduct,
                    nameProduct = sp.nameProduct,
                    sellPrice = sp.sellPrice,
                    Image = sp.Image,
                    idBranch = sp.idBranch,
                    idCategori = sp.idCategory,
                    nameBranch = sp.Branch.nameBranch,
                    nameCategory = sp.Category.nameCategory,
                    Quantity = sp.Quantity,
                    Description = sp.Description,
                    Logo = sp.Branch.Logo
                })
                .ToList();

            // Lấy danh sách ID sản phẩm đã lưu
            var savedProductIds = _context.SaveProducts
                .Where(s => s.idCustomer == customerId)
                .Select(s => s.idProduct)
                .ToList();

            // Gán danh sách cho ViewBag để sử dụng trong View
            ViewBag.SavedProductIds = savedProductIds;

            // Trả về PartialView với danh sách sản phẩm
            return PartialView("FilterProducts", products);
        }



        public string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public IActionResult SearchResults(string query)
        {
            // Chuẩn hóa từ khóa tìm kiếm
            string normalizedQuery = RemoveDiacritics(query.ToLower());

            // Lấy tất cả các sản phẩm trước, sau đó xử lý tìm kiếm trong bộ nhớ
            var products = _context.Products
                .Include(p => p.Branch)
                .Include(p => p.Category)
                .AsEnumerable()  // Chuyển sang client-side evaluation
                .Where(p => RemoveDiacritics(p.nameProduct.ToLower()).Contains(normalizedQuery))
                .Select(p => new ProductViewDto
                {
                    idProduct = p.idProduct,
                    nameProduct = p.nameProduct,
                    sellPrice = p.sellPrice,
                    Image = p.Image,
                    idBranch = p.idBranch,
                    idCategori = p.idCategory,
                    nameBranch = p.Branch.nameBranch,
                    nameCategory = p.Category.nameCategory,
                    Quantity = p.Quantity,
                    Description = p.Description,
                    Logo = p.Branch.Logo
                })
                .ToList();

            // Trả về View với danh sách sản phẩm tìm kiếm
            return View(products);
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
                var product = _context.Products.FirstOrDefault(p => p.idProduct == model.ProductId);
                // Kiểm tra đơn hàng hiện tại
                var order = _context.Orders.FirstOrDefault(o => o.idCustomer == idCustomer && o.statusOrder == OrderStatus.InCart);

                // Nếu không có đơn hàng hiện tại, tạo đơn hàng mới
                if (order == null)
                {
                    order = new Order
                    {
                        idOrder = GenerateOrderId(), // Gán idOrder với hàm GenerateOrderId
                        idCustomer = idCustomer,
                        statusOrder = OrderStatus.InCart
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
                    existingOrderDetail.totalPrice = existingOrderDetail.Quantity * product.sellPrice;  // Cập nhật tổng giá
                    _context.DetailOrders.Update(existingOrderDetail);
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
                        totalPrice = model.Quantity * product.sellPrice // Tính tổng giá
                    };

                    _context.DetailOrders.Add(newOrderDetail);
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

        public IActionResult ViewCart()
        {
            int idCustomer = GetCustomerIdFromSession();
            if (idCustomer == 0)
            {
                TempData["error"] = "Hãy đăng nhập.";
                return RedirectToAction("Login", "User");
            }

            var order = _context.Orders
                .Include(o => o.DetailOrders)
                
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.idCustomer == idCustomer && o.statusOrder == OrderStatus.InCart);

            // Kiểm tra nếu đơn hàng hoặc DetailOrders là null hoặc DetailOrders không có sản phẩm nào
            if (order == null || order.DetailOrders == null || !order.DetailOrders.Any())
            {
                ViewBag.Message = "Giỏ hàng của bạn đang trống.";
                return View();
            }

            // Tính toán tổng đơn hàng
            order.CalculateTotalOrder(); // Gọi phương thức tính tổng đơn hàng

            return View(order); // Truyền đơn hàng có trạng thái "InCart" vào View
        }
        public IActionResult MyOrder()
        {
            int idCustomer = GetCustomerIdFromSession();
            if (idCustomer == 0)
            {
                TempData["error"] = "Hãy đăng nhập.";
                return RedirectToAction("Login", "User");
            }

            // Lấy tất cả các giao dịch của khách hàng cùng với đơn hàng tương ứng
            var transactions = _context.Transactions
                .Include(t => t.Order)
                .ThenInclude(o => o.DetailOrders)
                .ThenInclude(d => d.Product)
                .Where(t => t.idCustomer == idCustomer )
                .ToList();

            // Kiểm tra nếu không có giao dịch nào
            if (transactions == null || !transactions.Any())
            {
                ViewBag.Message = "Bạn chưa có giao dịch nào.";
                return View();
            }

            return View(transactions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transaction(
     string name, string address, string phone, string email, string note,
     List<int> selectedDetailIds, string paymentType)
        {
            int idCustomer = GetCustomerIdFromSession();

            // Lấy giỏ hàng hiện tại
            var currentCart = _context.Orders
                .Include(o => o.DetailOrders)
                .FirstOrDefault(o => o.idCustomer == idCustomer && o.statusOrder == OrderStatus.InCart);

            if (currentCart == null)
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống.";
                return RedirectToAction("ViewCart");
            }

            // Tính tổng số tiền dựa trên các DetailOrder đã chọn
            decimal totalAmount = currentCart.DetailOrders
                .Where(d => selectedDetailIds.Contains(d.IdDetailOrder))
                .Sum(d => d.totalPrice ?? 0);

            // Tạo đơn hàng mới
            var newOrder = new Order
            {
                idOrder = GenerateOrderId(),
                idCustomer = idCustomer,
                statusOrder = OrderStatus.Pending,
                totalOrder = totalAmount
            };
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // Tạo một transaction từ dữ liệu người dùng đã nhập
            var transaction = new Transaction
            {
                nameCustomer = name,
                shippingAddress = address,
                phoneNumber = phone,
                Email = email,
                TotalAmount = totalAmount,
                createdDate = DateTime.Now,
                cpdatedDate = DateTime.Now,
                Note = note,
                idCustomer = idCustomer,
                idOrder = newOrder.idOrder
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            // Lọc các DetailOrder trong giỏ hàng mà người dùng đã chọn
            var selectedDetails = currentCart.DetailOrders
                .Where(d => selectedDetailIds.Contains(d.IdDetailOrder))
                .ToList();

            foreach (var detail in selectedDetails)
            {
                var newOrderDetail = new DetailOrder
                {
                    idOrder = newOrder.idOrder,
                    idProduct = detail.idProduct,
                    Quantity = detail.Quantity,
                    nameColor = detail.nameColor,
                    nameSize = detail.nameSize,
                    totalPrice = detail.totalPrice
                };
                _context.DetailOrders.Add(newOrderDetail);
            }

            // Xóa các DetailOrder đã chuyển đi
            _context.DetailOrders.RemoveRange(selectedDetails);
            _context.SaveChanges();

            // Thêm thông tin thanh toán cho giao dịch
            if (paymentType == "immediate")
            {
                var payment = new Payment
                {
                    idTransaction = transaction.idTransaction,
                    Amount = totalAmount,
                    CreatedDate = DateTime.Now,
                    QRCodeUrl = GenerateQRCodeUrl(transaction.idOrder, totalAmount, transaction.nameCustomer)
                };
                _context.Payments.Add(payment);
                _context.SaveChanges();

                TempData["success"] = "Đặt hàng thành công!";
                return RedirectToAction("ShowQRCode", new { paymentId = payment.Id });
            }
            else if (paymentType == "later")
            {
                TempData["success"] = "Đặt hàng thành công! Bạn có thể thanh toán sau.";
                return RedirectToAction("MyOrder");
            }

            return RedirectToAction("ViewCart");
        }

        private string GenerateQRCodeUrl(string idorder, decimal amount, string customerName)
        {
            // Thông tin tài khoản ngân hàng
            string bankCode = "MB"; // Thay bằng mã ngân hàng của bạn
            string accountNumber = "0000856986704"; // Thay bằng số tài khoản của bạn

            // Tạo mô tả giao dịch bao gồm ID đơn hàng và tên khách hàng
            string transactionDescription = $"{customerName} thanh toán đơn hàng {idorder} ";

            // Tạo URL QR code tích hợp tài khoản ngân hàng và thông tin thanh toán
            string qrUrl = $"https://img.vietqr.io/image/{bankCode}-{accountNumber}-qr_only.png?amount={amount}&addInfo={Uri.EscapeDataString(transactionDescription)}";

            return qrUrl;
        }


        public IActionResult ShowQRCode(int paymentId)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == paymentId);

            if (payment == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin thanh toán.";
                return RedirectToAction("ViewCart");
            }

            // Tải dữ liệu giao dịch liên quan nếu cần
            var transaction = _context.Transactions
                .Include(t => t.Order)
                .ThenInclude(t => t.DetailOrders)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(t => t.idTransaction == payment.idTransaction);
            if (transaction != null)
            {
                ViewBag.Transaction = transaction;
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin giao dịch.";
                return RedirectToAction("ViewCart");
            }

            return View(payment);
        }

        [HttpPost]
        public IActionResult PaymentNotification(PaymentNotificationDto notification)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.idTransaction == notification.TransactionId);
            // Tải dữ liệu giao dịch liên quan nếu cần
            var transaction = _context.Transactions
                .Include(t => t.Order)
                .ThenInclude(t => t.DetailOrders)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(t => t.idTransaction == payment.idTransaction);

            if (payment != null && notification.IsSuccess)
            {
                TempData["success"] = "Thanh toán thành công!";
                transaction.Order.statusPay = PaymentStatus.Paid;
                _context.SaveChanges();

                // Thông báo cho người dùng
              
            }

            return Ok(); // Trả về phản hồi cho dịch vụ thanh toán
        }

        private void NotifyUser(Payment payment)
        {
            // Gửi thông báo cho người dùng về việc thanh toán thành công
            // Ví dụ: gửi email hoặc tin nhắn
        }





        [HttpPost]
        public IActionResult DeleteDetailOrder(int id)
        {
            // Kiểm tra id không hợp lệ
            if (id <= 0)
            {
                return BadRequest("ID không hợp lệ.");
            }

            // Tìm DetailOrder trong cơ sở dữ liệu
            var detail = _context.DetailOrders.Find(id);
            if (detail == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            try
            {
                // Lấy idOrder từ DetailOrder trước khi xóa
                var orderId = detail.idOrder;

                // Xóa sản phẩm khỏi giỏ hàng
                _context.DetailOrders.Remove(detail);
                _context.SaveChanges();

                // Kiểm tra xem Order có còn DetailOrder nào không
                var remainingDetails = _context.DetailOrders
                    .Any(d => d.idOrder == orderId);

                if (!remainingDetails)
                {
                    // Nếu không còn DetailOrder nào, xóa Order và Transaction liên quan
                    var order = _context.Orders.FirstOrDefault(o => o.idOrder == orderId);
                    if (order != null)
                    {
                        // Tìm Transaction liên quan đến Order này
                        var transaction = _context.Transactions
                            .FirstOrDefault(t => t.idOrder == orderId);

                        if (transaction != null)
                        {
                            _context.Transactions.Remove(transaction);
                        }

                        _context.Orders.Remove(order);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    // Nếu vẫn còn DetailOrder, cập nhật lại totalOrder cho Order và Transaction
                    decimal newTotalOrder = _context.DetailOrders
                        .Where(d => d.idOrder == orderId)
                        .Sum(d => d.totalPrice ?? 0);

                    var order = _context.Orders.FirstOrDefault(o => o.idOrder == orderId);
                    if (order != null)
                    {
                        order.totalOrder = newTotalOrder;
                    }

                    var transaction = _context.Transactions
                        .FirstOrDefault(t => t.idOrder == orderId);
                    if (transaction != null)
                    {
                        transaction.TotalAmount = newTotalOrder;
                    }

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi khi xóa
                return StatusCode(500, $"Đã xảy ra lỗi khi xóa sản phẩm: {ex.Message}");
            }

            // Trả về thông báo thành công
            return Ok("Sản phẩm đã được xóa thành công và cập nhật đơn hàng.");
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
                ViewBag.Message = "Bạn chưa lưu sản phẩm nào nào.";
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
                return Json(new { success = true, message = "Product has been deleted successfully!" });
            }

            return Json(new { success = false, message = "Saved product not found!" });
        }





    }
}
