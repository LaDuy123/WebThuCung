using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebThuCung.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebThuCung.Models;
using WebThuCung.Dto;
using Microsoft.EntityFrameworkCore;

namespace WebThuCung.Controllers
{
    public class AdminController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public AdminController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index(string period = "today")
        {
            // Lấy dữ liệu bán hàng (số đơn hàng theo ngày, tuần, tháng)
            var salesData = GetSalesData(period);
            var revenueData = GetRevenueData(period);
            var customerData = GetCustomerData(period);
            var topSellingData = GetTopSellingProducts(period);

            var model = new DashboardViewDto
            {
                Sales = salesData,
                Revenue = revenueData,
                Customer = customerData,
                TopSellingProducts = topSellingData
            };


            ViewBag.SelectedPeriod = period; // Để theo dõi khoảng thời gian được chọn

            // Trả về view Index với model đã khởi tạo
            return View(model);
        }


        private SalesViewDto GetSalesData(string period)
        {
            // Lấy ngày hôm nay
            var today = DateTime.Today;

            var salesViewModel = new SalesViewDto();

            switch (period.ToLower())
            {
                case "week":
                    salesViewModel = GetWeeklySalesData(today);
                    break;
                case "month":
                    salesViewModel = GetMonthlySalesData(today);
                    break;
                case "today":
                default:
                    salesViewModel = GetDailySalesData(today);
                    break;
            }

            return salesViewModel;
        }
        private RevenueViewDto GetRevenueData(string period)
        {
            // Lấy ngày hôm nay
            var today = DateTime.Today;

            var revenueViewModel = new RevenueViewDto();

            switch (period.ToLower())
            {
                case "week":
                    revenueViewModel = GetWeeklyRevenueData(today);
                    break;
                case "month":
                    revenueViewModel = GetMonthlyRevenueData(today);
                    break;
                case "today":
                default:
                    revenueViewModel = GetDailyRevenueData(today);
                    break;
            }

            return revenueViewModel;
        }
        private CustomerViewDto GetCustomerData(string period)
        {
            // Lấy ngày hôm nay
            var today = DateTime.Today;

            var customerViewModel = new CustomerViewDto();

            switch (period.ToLower())
            {
                case "week":
                    customerViewModel = GetWeeklyCustomerData(today);
                    break;
                case "month":
                    customerViewModel = GetMonthlyCustomerData(today);
                    break;
                case "today":
                default:
                    customerViewModel = GetDailyCustomerData(today);
                    break;
            }

            return customerViewModel;
        }
        private List<TopSellingProductDto> GetTopSellingProducts(string period)
        {
            DateTime today = DateTime.Today;
            DateTime startDate, endDate;

            switch (period)
            {
                case "today":
                    startDate = today;
                    endDate = today;
                    break;
                case "week":
                    startDate = today.AddDays(-(int)today.DayOfWeek); 
                    endDate = startDate.AddDays(6);
                    break;
                case "month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                default:
                    startDate = today;
                    endDate = today;
                    break;
            }

            return _context.Orders
                .Where(o => o.dateFrom.Date >= startDate && o.dateFrom.Date <= endDate)
                .SelectMany(o => o.DetailOrders)
                .GroupBy(d => new { d.idProduct, d.Product.nameProduct, d.Product.sellPrice, d.Product.Image })
                .Select(g => new TopSellingProductDto
                {
                    ProductId = g.Key.idProduct,
                    ProductName = g.Key.nameProduct,
                    Price = g.Key.sellPrice,
                    Sold = g.Sum(d => d.Quantity),
                    Revenue = g.Sum(d => d.Product.sellPrice * d.Quantity),
                    ImageUrl = g.Key.Image
                })
                .OrderByDescending(p => p.Sold)
                .Take(5) // Lấy 5 sản phẩm bán chạy nhất
                .ToList();
        }
        private CustomerViewDto GetDailyCustomerData(DateTime today)
        {
            int totalCustomerToday = _context.Customers
                                           .Where(o => o.createdAt.Date == today)
                                           .Count();

            var yesterday = today.AddDays(-1);
            int totalCustomerYesterday = _context.Customers
                                               .Where(o => o.createdAt.Date == yesterday)
                                               .Count();

            double growthPercentageDay = CalculateGrowthPercentage(totalCustomerToday, totalCustomerYesterday);

            return new CustomerViewDto
            {
                TotalCustomerToday = totalCustomerToday,
                GrowthPercentageDay = growthPercentageDay
            };
        }

        private CustomerViewDto GetWeeklyCustomerData(DateTime today)
        {
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Ngày đầu tuần (Chủ Nhật)
            var endOfWeek = startOfWeek.AddDays(6); // Ngày cuối tuần (Thứ Bảy)

            int totalCustomerThisWeek = _context.Customers
                                              .Where(o => o.createdAt.Date >= startOfWeek && o.createdAt.Date <= endOfWeek)
                                              .Count();

            var startOfLastWeek = startOfWeek.AddDays(-7);
            var endOfLastWeek = endOfWeek.AddDays(-7);

            int totalCusstomerLastWeek = _context.Customers
                                              .Where(o => o.createdAt.Date >= startOfLastWeek && o.createdAt.Date <= endOfLastWeek)
                                              .Count();

            double growthPercentageWeek = CalculateGrowthPercentage(totalCustomerThisWeek, totalCusstomerLastWeek);

            return new CustomerViewDto
            {
                TotalCustomerThisWeek = totalCustomerThisWeek,
                GrowthPercentageWeek = growthPercentageWeek
            };
        }

        private CustomerViewDto GetMonthlyCustomerData(DateTime today)
        {
            var startOfMonth = new DateTime(today.Year, today.Month, 1); // Ngày đầu tháng
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // Ngày cuối tháng

            int totalCustomerThisMonth = _context.Customers
                                               .Where(o => o.createdAt.Date >= startOfMonth && o.createdAt.Date <= endOfMonth)
                                               .Count();

            var startOfLastMonth = startOfMonth.AddMonths(-1);
            var endOfLastMonth = startOfLastMonth.AddMonths(1).AddDays(-1); // Ngày cuối tháng trước

            int totalCustomerLastMonth = _context.Customers
                                               .Where(o => o.createdAt.Date >= startOfLastMonth && o.createdAt.Date <= endOfLastMonth)
                                               .Count();

            double growthPercentageMonth = CalculateGrowthPercentage(totalCustomerThisMonth, totalCustomerLastMonth);

            return new CustomerViewDto
            {
                TotalCustomerThisMonth = totalCustomerThisMonth,
                GrowthPercentageMonth = growthPercentageMonth
            };
        }

        private SalesViewDto GetDailySalesData(DateTime today)
        {
            int totalOrdersToday = _context.Orders
                                           .Where(o => o.dateFrom.Date == today)
                                           .Count();

            var yesterday = today.AddDays(-1);
            int totalOrdersYesterday = _context.Orders
                                               .Where(o => o.dateFrom.Date == yesterday)
                                               .Count();

            double growthPercentageDay = CalculateGrowthPercentage(totalOrdersToday, totalOrdersYesterday);

            return new SalesViewDto
            {
                TotalOrdersToday = totalOrdersToday,
                GrowthPercentageDay = growthPercentageDay
            };
        }

        private SalesViewDto GetWeeklySalesData(DateTime today)
        {
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Ngày đầu tuần (Chủ Nhật)
            var endOfWeek = startOfWeek.AddDays(6); // Ngày cuối tuần (Thứ Bảy)

            int totalOrdersThisWeek = _context.Orders
                                              .Where(o => o.dateFrom.Date >= startOfWeek && o.dateFrom.Date <= endOfWeek)
                                              .Count();

            var startOfLastWeek = startOfWeek.AddDays(-7);
            var endOfLastWeek = endOfWeek.AddDays(-7);

            int totalOrdersLastWeek = _context.Orders
                                              .Where(o => o.dateFrom.Date >= startOfLastWeek && o.dateFrom.Date <= endOfLastWeek)
                                              .Count();

            double growthPercentageWeek = CalculateGrowthPercentage(totalOrdersThisWeek, totalOrdersLastWeek);

            return new SalesViewDto
            {
                TotalOrdersThisWeek = totalOrdersThisWeek,
                GrowthPercentageWeek = growthPercentageWeek
            };
        }

        private SalesViewDto GetMonthlySalesData(DateTime today)
        {
            var startOfMonth = new DateTime(today.Year, today.Month, 1); // Ngày đầu tháng
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // Ngày cuối tháng

            int totalOrdersThisMonth = _context.Orders
                                               .Where(o => o.dateFrom.Date >= startOfMonth && o.dateFrom.Date <= endOfMonth)
                                               .Count();

            var startOfLastMonth = startOfMonth.AddMonths(-1);
            var endOfLastMonth = startOfLastMonth.AddMonths(1).AddDays(-1); // Ngày cuối tháng trước

            int totalOrdersLastMonth = _context.Orders
                                               .Where(o => o.dateFrom.Date >= startOfLastMonth && o.dateFrom.Date <= endOfLastMonth)
                                               .Count();

            double growthPercentageMonth = CalculateGrowthPercentage(totalOrdersThisMonth, totalOrdersLastMonth);

            return new SalesViewDto
            {
                TotalOrdersThisMonth = totalOrdersThisMonth,
                GrowthPercentageMonth = growthPercentageMonth
            };
        }

       

        private RevenueViewDto GetDailyRevenueData(DateTime today)
        {
            var ordersToday = _context.Orders
     .Where(o => o.dateFrom.Date == today)
     .Include(o => o.DetailOrders)
     .ThenInclude(p => p.Product)// Bao gồm cả DetailOrders
     .ToList();  // Lấy toàn bộ dữ liệu ra phía client

            decimal totalRevenueToday = ordersToday
                .Where(o => o.DetailOrders != null && o.DetailOrders.Any())
                .Sum(o => o.DetailOrders.Sum(d => d.Product.sellPrice * d.Quantity));

            var yesterday = today.AddDays(-1);
            var ordersYesterday = _context.Orders
     .Where(o => o.dateFrom.Date == yesterday)
     .Include(o => o.DetailOrders)
      .ThenInclude(p => p.Product)// Bao gồm DetailOrders trong kết quả
     .ToList();  // Lấy toàn bộ đơn hàng của ngày hôm qua về phía client

            decimal totalRevenueYesterday = ordersYesterday
                .Where(o => o.DetailOrders != null && o.DetailOrders.Any())  // Lọc các đơn hàng có DetailOrders
                 .Sum(o => o.DetailOrders.Sum(d => d.Product.sellPrice * d.Quantity)); // Tính tổng doanh thu của các DetailOrders

            double growthPercentageDay = CalculateGrowthPercentage1(totalRevenueToday, totalRevenueYesterday);

            return new RevenueViewDto
            {
                TotalRevenueToday = totalRevenueToday,           
                GrowthPercentageDay = growthPercentageDay
            };
        }

        private RevenueViewDto GetWeeklyRevenueData(DateTime today)
        {
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Ngày đầu tuần (Chủ Nhật)
            var endOfWeek = startOfWeek.AddDays(6); // Ngày cuối tuần (Thứ Bảy)

            var ordersThisWeek = _context.Orders
    .Where(o => o.dateFrom.Date >= startOfWeek && o.dateFrom.Date <= endOfWeek)
    .Include(o => o.DetailOrders)
     .ThenInclude(p => p.Product)// Bao gồm DetailOrders trong kết quả
    .ToList();  // Lấy toàn bộ đơn hàng của tuần về phía client

            decimal totalRevenueThisWeek = ordersThisWeek
                .Where(o => o.DetailOrders != null && o.DetailOrders.Any())  // Lọc các đơn hàng có DetailOrders
               .Sum(o => o.DetailOrders.Sum(d => d.Product.sellPrice * d.Quantity));  // Tính tổng doanh thu của các DetailOrders


            var startOfLastWeek = startOfWeek.AddDays(-7);
            var endOfLastWeek = endOfWeek.AddDays(-7);

            var ordersLastWeek = _context.Orders
    .Where(o => o.dateFrom.Date >= startOfLastWeek && o.dateFrom.Date <= endOfLastWeek)
    .Include(o => o.DetailOrders)
     .ThenInclude(p => p.Product)// Bao gồm DetailOrders trong kết quả
    .ToList();  // Lấy toàn bộ đơn hàng của tuần trước về phía client

            decimal totalRevenueLastWeek = ordersLastWeek
                .Where(o => o.DetailOrders != null && o.DetailOrders.Any())  // Lọc các đơn hàng có DetailOrders
                .Sum(o => o.DetailOrders.Sum(d => d.Product.sellPrice * d.Quantity)); // Tính tổng doanh thu của các DetailOrders


            double growthPercentageWeek = CalculateGrowthPercentage1(totalRevenueThisWeek, totalRevenueLastWeek);

            return new RevenueViewDto
            {
                TotalRevenueThisWeek = totalRevenueThisWeek,
                GrowthPercentageWeek = growthPercentageWeek
            };
        }

        private RevenueViewDto GetMonthlyRevenueData(DateTime today)
        {
            var startOfMonth = new DateTime(today.Year, today.Month, 1); // Ngày đầu tháng
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // Ngày cuối tháng

            var ordersThisMonth = _context.Orders
    .Where(o => o.dateFrom.Month == today.Month && o.dateFrom.Year == today.Year)
    .Include(o => o.DetailOrders)
     .ThenInclude(p => p.Product)// Bao gồm DetailOrders trong kết quả
    .ToList();  // Lấy toàn bộ đơn hàng của tháng hiện tại về phía client

            decimal totalRevenueThisMonth = ordersThisMonth
                .Where(o => o.DetailOrders != null && o.DetailOrders.Any())  // Lọc các đơn hàng có DetailOrders
               .Sum(o => o.DetailOrders.Sum(d => d.Product.sellPrice * d.Quantity));  // Tính tổng doanh thu của các DetailOrders


            var startOfLastMonth = startOfMonth.AddMonths(-1);
            var endOfLastMonth = startOfLastMonth.AddMonths(1).AddDays(-1); // Ngày cuối tháng trước

           var ordersLastMonth = _context.Orders
    .Where(o => o.dateFrom >= startOfLastMonth && o.dateFrom <= endOfLastMonth)
    .Include(o => o.DetailOrders)
     .ThenInclude(p => p.Product)// Bao gồm DetailOrders trong kết quả
    .ToList();  // Lấy toàn bộ đơn hàng của tháng trước về phía client

decimal totalRevenueLastMonth = ordersLastMonth
    .Where(o => o.DetailOrders != null && o.DetailOrders.Any())  // Lọc các đơn hàng có DetailOrders
    .Sum(o => o.DetailOrders.Sum(d => d.Product.sellPrice * d.Quantity));  // Tính tổng doanh thu của các DetailOrders

            double growthPercentageMonth = CalculateGrowthPercentage1(totalRevenueThisMonth, totalRevenueLastMonth);

            return new RevenueViewDto
            {
                TotalRevenueThisMonth = totalRevenueThisMonth,
                GrowthPercentageMonth = growthPercentageMonth,
            };
        }


        private double CalculateGrowthPercentage(int current, int previous)
        {
            if (previous == 0) return current > 0 ? 100 : 0; // Tránh chia cho 0
            return ((double)(current - previous) / previous) * 100;
        }
        private double CalculateGrowthPercentage1(decimal current, decimal previous)
        {
            if (previous == 0) return current > 0 ? 100 : 0; // Tránh chia cho 0
                                                             // Chuyển đổi 'previous' thành 'double' trước khi chia
            return ((double)(current - previous) / (double)previous) * 100;
        }

        // GET: Hiển thị trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                // Tìm kiếm admin trong cơ sở dữ liệu
                var ad = _context.Admins.SingleOrDefault(n => n.userAdmin == model.userName && n.passwordAdmin == model.password);

                if (ad != null)
                {
                    TempData["success"] = "Đăng nhập thành công";
                    // Serialize thông tin khách hàng thành JSON và lưu vào Session
                    var adminJson = Newtonsoft.Json.JsonConvert.SerializeObject(ad);
                    HttpContext.Session.SetString("TaikhoanAdmin", adminJson);
                    HttpContext.Session.SetString("email", ad.Email);

               

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View(model);
        }
        public IActionResult ListAdmin()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var admins = _context.Admins.ToList();

            // Truyền danh sách admin sang view
            return View(admins);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("TaikhoanAdmin");

            ViewBag.Thongbao = "Bạn đã đăng xuất thành công.";

            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public IActionResult CheckAuthentication()
        {
            string email = HttpContext.Session.GetString("email");

            if (!string.IsNullOrEmpty(email))
            {
                // Kiểm tra bảng customer
                var admin = _context.Admins.FirstOrDefault(c => c.Email == email);
                if (admin != null)
                {
                    return new JsonResult(new
                    {
                        isAuthenticated = true,
                        isadmin = true,
                        avatar = admin.Avatar// Giả sử bạn có trường Avatar trong customer
                    });
                }

                // Kiểm tra bảng Recruiter

            }

            // Nếu không tìm thấy email hoặc người dùng chưa đăng nhập
            return new JsonResult(new { isAuthenticated = false });
        }

    }
}
