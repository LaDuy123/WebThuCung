using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebThuCung.Data;
using WebThuCung.Dto;
using WebThuCung.Models;

namespace WebThuCung.Controllers
{
    public class OrderController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public OrderController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các đơn hàng từ cơ sở dữ liệu
            var orders = _context.Orders
                .Include(o => o.DetailOrders)
                .ThenInclude(p => p.Product)
                .ToList();

            // Tính toán tổng giá trị cho mỗi đơn hàng
            foreach (var order in orders)
            {
                order.CalculateTotalOrder(); // Calculate total order based on DetailOrders
            }

            // Truyền danh sách đơn hàng sang view
            return View(orders);
        }
        [Authorize(Roles = "Admin,StaffOrder")]
        // GET: Order/Edit/{id}
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var order = _context.Orders
                                .Include(o => o.Customer) // Kết hợp thông tin khách hàng
                                .Include(o => o.DetailOrders) // Kết hợp thông tin chi tiết đơn hàng
                                .FirstOrDefault(o => o.idOrder == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderDto
            {
                idOrder = order.idOrder,
                idCustomer = order.idCustomer,
                dateTo = order.dateTo,
                statusOrder = order.statusOrder,
                statusPay = order.statusPay,
                totalOrder = order.totalOrder
            };

            // Truyền danh sách khách hàng vào ViewBag
            ViewBag.Customers = _context.Customers.Select(c => new SelectListItem
            {
                Value = c.idCustomer.ToString(),
                Text = c.nameCustomer // Giả sử bạn có thuộc tính nameCustomer trong Customer
            }).ToList();

            return View(orderDto);
        }

        // POST: Order/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderDto orderDto)
        {
            ViewBag.Customers = _context.Customers.Select(c => new SelectListItem
            {
                Value = c.idCustomer.ToString(),
                Text = c.nameCustomer
            }).ToList();

            if (ModelState.IsValid)
            {
                var order = _context.Orders.FirstOrDefault(o => o.idOrder == orderDto.idOrder);
                if (order == null)
                {
                    return NotFound();
                }

                // Cập nhật các giá trị từ DTO vào model
                order.idCustomer = orderDto.idCustomer;
                order.dateTo = orderDto.dateTo;
                order.statusOrder = orderDto.statusOrder;
                order.statusPay = orderDto.statusPay;

                // Tính toán tổng đơn hàng nếu có thay đổi trong chi tiết đơn hàng
                order.CalculateTotalOrder(); // Gọi phương thức để tính toán tổng giá trị của đơn hàng

                _context.SaveChanges();
                return RedirectToAction("Index"); // Quay lại trang danh sách đơn hàng sau khi cập nhật
            }

            return View(orderDto);
        }
        [Authorize(Roles = "Admin,StaffOrder")]
        // POST: Order/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Quay lại danh sách đơn hàng sau khi xóa
        }
        public IActionResult Detail(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return NotFound();
            }

            // Lấy danh sách các DetailOrders cho một đơn hàng cụ thể và bao gồm thông tin sản phẩm liên quan
            var detailOrders = _context.DetailOrders
                .Include(d => d.Product)
                .Where(d => d.idOrder == orderId)
                .ToList();

            // Tính tổng giá cho mỗi DetailOrder
            foreach (var detailOrder in detailOrders)
            {
                detailOrder.totalPrice = detailOrder.CalculateTotalPrice();
            }

            // Truyền orderId vào view để liên kết trở lại đơn hàng
            ViewBag.OrderId = orderId;

            return View(detailOrders); // Trả về view detail cho DetailOrders
        }
        [HttpGet]
        public IActionResult CreateDetail(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return NotFound();
            }

            // Truyền orderId sang view
            ViewBag.OrderId = orderId;

            // Truyền danh sách sản phẩm để chọn lựa
            ViewBag.Products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct.ToString(),
                Text = p.nameProduct
            }).ToList();
            ViewBag.Sizes = _context.Sizes.Select(s => new SelectListItem
            {
                Value = s.idSize.ToString(),
                Text = s.nameSize
            }).ToList();
            ViewBag.Colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.idColor.ToString(),
                Text = c.nameColor
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDetail(DetailOrderDto detailOrderDto)
        {
            ViewBag.Products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct.ToString(),
                Text = p.nameProduct
            }).ToList();
            ViewBag.Sizes = _context.Sizes.Select(s => new SelectListItem
            {
                Value = s.idSize.ToString(),
                Text = s.nameSize
            }).ToList();
            ViewBag.Colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.idColor.ToString(),
                Text = c.nameColor
            }).ToList();
            if (ModelState.IsValid)
            {
                // Kiểm tra xem sản phẩm đã tồn tại trong đơn hàng hay chưa
                var existingDetailOrder = _context.DetailOrders
                    .Include(d => d.Product)
                    .FirstOrDefault(d => d.idOrder == detailOrderDto.idOrder && d.idProduct == detailOrderDto.idProduct);

                if (existingDetailOrder != null)
                {
                    // Nếu sản phẩm đã tồn tại, thêm lỗi vào ModelState và trả về form
                    ModelState.AddModelError("idProduct", "Sản phẩm đã tồn tại trong đơn hàng này.");
                }
                else
                {
                    var product = _context.Products.FirstOrDefault(p => p.idProduct == detailOrderDto.idProduct);
                    // Chuyển đổi từ DTO sang model DetailOrder
                    var detailOrder = new DetailOrder
                    {
                        idOrder = detailOrderDto.idOrder,
                        idProduct = detailOrderDto.idProduct,
                        //idColor = detailOrderDto.idColor,
                        //idSize = detailOrderDto.idSize,
                        Quantity = detailOrderDto.Quantity,
                        totalPrice = detailOrderDto.Quantity * product.sellPrice // Tính tổng giá
                    };

                    _context.DetailOrders.Add(detailOrder);
                    _context.SaveChanges();

                    // Chuyển hướng về danh sách DetailOrder của đơn hàng cụ thể
                    return RedirectToAction("Detail", new { orderId = detailOrder.idOrder });
                }
            }

            // Nếu không hợp lệ, tải lại form
            ViewBag.OrderId = detailOrderDto.idOrder;
        

            return View(detailOrderDto);
        }



        // GET: DetailOrder/Edit/{orderId}/{productId}
        [HttpGet]
        public IActionResult EditDetail(string orderId, string productId)
        {
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(productId))
            {
                return NotFound();
            }

            var detailOrder = _context.DetailOrders
                .FirstOrDefault(d => d.idOrder == orderId && d.idProduct == productId);

            if (detailOrder == null)
            {
                return NotFound();
            }
            //var product = _context.Products.FirstOrDefault(p => p.idProduct == detailOrderDto.idProduct);
            var detailOrderDto = new DetailOrderDto
            {
                idOrder = detailOrder.idOrder,
                idProduct = detailOrder.idProduct,
                Quantity = detailOrder.Quantity,          
                totalPrice = detailOrder.CalculateTotalPrice()
            };

            ViewBag.Products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct.ToString(),
                Text = p.nameProduct
            }).ToList();
            ViewBag.Sizes = _context.Sizes.Select(s => new SelectListItem
            {
                Value = s.idSize.ToString(),
                Text = s.nameSize
            }).ToList();
            ViewBag.Colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.idColor.ToString(),
                Text = c.nameColor
            }).ToList();

            return View(detailOrderDto);
        }

        // POST: DetailOrder/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDetail(DetailOrderDto detailOrderDto)
        {
            if (ModelState.IsValid)
            {
                var detailOrder = _context.DetailOrders
                    .FirstOrDefault(d => d.idOrder == detailOrderDto.idOrder && d.idProduct == detailOrderDto.idProduct);

                if (detailOrder == null)
                {
                    return NotFound();
                }
                var product = _context.Products.FirstOrDefault(p => p.idProduct == detailOrderDto.idProduct);
                detailOrder.Quantity = detailOrderDto.Quantity;
                detailOrder.totalPrice = detailOrderDto.Quantity * product.sellPrice;

                _context.SaveChanges();

                return RedirectToAction("Detail", new { orderId = detailOrder.idOrder });
            }

            ViewBag.Products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.idProduct.ToString(),
                Text = p.nameProduct
            }).ToList();
            ViewBag.Sizes = _context.Sizes.Select(s => new SelectListItem
            {
                Value = s.idSize.ToString(),
                Text = s.nameSize
            }).ToList();
            ViewBag.Colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.idColor.ToString(),
                Text = c.nameColor
            }).ToList();

            return View(detailOrderDto);
        }
        // POST: DetailOrder/Delete/{orderId}/{productId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDetail(string orderId, string productId)
        {
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(productId))
            {
                return NotFound();
            }

            // Fetch the detail order to delete
            var detailOrder = _context.DetailOrders
                .FirstOrDefault(d => d.idOrder == orderId && d.idProduct == productId);

            if (detailOrder == null)
            {
                return NotFound();
            }

            _context.DetailOrders.Remove(detailOrder);
            _context.SaveChanges();

            return RedirectToAction("Detail", new { orderId });
        }

    }
}
