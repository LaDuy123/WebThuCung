using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebThuCung.Data;
using WebThuCung.Dto;
using WebThuCung.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebThuCung.Controllers
{
    public class UserController : Controller
    {
        private readonly PetContext _context;
        public UserController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Truy vấn lấy các sản phẩm cùng các thuộc tính liên quan sử dụng Include
            var allProducts = _context.Products
                                 .Include(sp => sp.Branch)  // Bao gồm thông tin thương hiệu
                                 .Include(sp => sp.Category)        // Bao gồm thông tin loại
                                 .Include(sp => sp.Color)      // Bao gồm thông tin màu sắc
                                 .Select(sp => new ProductViewDto
                                 {
                                     idProduct = sp.idProduct,
                                     nameProduct = sp.nameProduct,
                                     sellPrice = sp.sellPrice,
                                     Image = sp.Image,
                                     idBranch = sp.idBranch,
                                     idCategori = sp.idCategory,
                                     nameBranch = sp.Branch.namBranch,   // Lấy tên thương hiệu từ đối tượng liên quan
                                     nameCategory = sp.Category.namCategory,     // Lấy tên loại từ đối tượng liên quan
                                     Quantity = sp.Quantity,
                                     Description = sp.Description,
                                     nameColor = sp.Color.nameColor, // Lấy tên màu sắc từ đối tượng liên quan
                                     Logo = sp.Branch.Logo     // Lấy logo từ đối tượng liên quan
                                 })
                                 .OrderBy(p => p.idProduct)
                                 .Take(6)  // Lấy ra 6 sản phẩm
                                 .ToList();

            return View(allProducts);
        }
    
        #region Kiểm tra tên đăng nhập đã tồn tại
        public bool KiemTraTenDn(string tendn)
        {
            return _context.Customers.Any(x => x.userCustomer == tendn);
        }
        #endregion

        public bool KiemTraEmail(string email)
        {
            return _context.Customers.Any(x => x.Email == email);
        }

        // Hàm sử dụng BCrypt để băm mật khẩu
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        //#region Đăng ký tài khoản người dùng
        [HttpPost]
        public IActionResult Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập hoặc email đã tồn tại chưa
                if (KiemTraTenDn(model.userCustomer))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (KiemTraEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    // Tạo đối tượng KHACHHANG và gán thông tin từ form đăng ký
                    var khachHang = new Customer
                    {
                        nameCustomer = model.Name,
                        userCustomer = model.userCustomer,
                        Email = model.Email,
                        Address = model.Address,
                        Phone = model.Phone,
                        Image = model.Image,
                        dateBirth = model.DateBirth
                    };

                    // Sử dụng BCrypt để băm mật khẩu
                    khachHang.passwordCustomer = HashPassword(model.paswordCusstomer);

                    // Thêm khách hàng mới vào cơ sở dữ liệu
                    _context.Customers.Add(khachHang);
                    _context.SaveChanges();

                    // Sau khi đăng ký thành công, chuyển hướng người dùng đến trang đăng nhập
                    return RedirectToAction("Login");
                }
            }

            // Trả về view với dữ liệu của model khi có lỗi
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                // Tìm kiếm khách hàng theo tên đăng nhập
                var customer = _context.Customers.SingleOrDefault(n => n.userCustomer == model.userCustomer);

                // Kiểm tra nếu tài khoản tồn tại
                if (customer != null)
                {
                    // Kiểm tra mật khẩu
                    if (BCrypt.Net.BCrypt.Verify(model.passwordCustomer, customer.passwordCustomer))
                    {
                        // Đăng nhập thành công
                        ViewBag.Thongbao = "Đăng nhập thành công";

                        // Lưu thông tin đăng nhập vào Session
                        HttpContext.Session.SetString("Taikhoan", customer.nameCustomer);

                        // Chuyển hướng về trang chính sau khi đăng nhập thành công
                        return RedirectToAction("Index", "User");
                    }
                }

                // Nếu tên đăng nhập hoặc mật khẩu không đúng
                ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            else
            {
                // Xử lý nếu model không hợp lệ
                ViewBag.Thongbao = "Dữ liệu không hợp lệ.";
            }

            // Trả về view với model để hiển thị thông báo
            return View(model);
        }

    }
}
