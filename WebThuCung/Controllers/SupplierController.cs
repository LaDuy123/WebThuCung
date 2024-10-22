using Microsoft.AspNetCore.Mvc;
using WebThuCung.Data;
using WebThuCung.Dto;
using WebThuCung.Models;

namespace WebThuCung.Controllers
{
    public class SupplierController : Controller
    {
        private readonly PetContext _context; // Biến để truy cập cơ sở dữ liệu

        public SupplierController(PetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy danh sách các admin từ cơ sở dữ liệu
            var suppliers = _context.Suppliers.ToList();

            // Truyền danh sách admin sang view
            return View(suppliers);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SupplierDto supplierDto)
        {
            if (ModelState.IsValid)
            {
                var existingColor = _context.Suppliers.FirstOrDefault(s => s.idSupplier == supplierDto.idSupplier);
                if (existingColor != null)
                {
                    ModelState.AddModelError("", $"Color with ID '{supplierDto.idSupplier}' already exists.");
                    return View(supplierDto); // Trả lại form với thông báo lỗi
                }
                // Chuyển đổi DTO sang model Supplier
                var supplier = new Supplier
                {
                    idSupplier =supplierDto.idSupplier,
                    nameSupplier = supplierDto.NameSupplier,
                    Phone = supplierDto.Phone,
                    Address = supplierDto.Address,
                    Email = supplierDto.Email,
                    Image = supplierDto.Image
                };

                _context.Suppliers.Add(supplier);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Quay lại danh sách nhà cung cấp
            }

            return View(supplierDto);
        }

        // GET: Supplier/Edit/{id}
        public IActionResult Edit(string id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.idSupplier == id);
            if (supplier == null)
            {
                return NotFound();
            }

            var supplierDto = new SupplierDto
            {
                idSupplier = supplier.idSupplier,
                NameSupplier = supplier.nameSupplier,
                Phone = supplier.Phone,
                Address = supplier.Address,
                Email = supplier.Email,
                Image = supplier.Image
            };

            return View(supplierDto);
        }

        // POST: Supplier/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SupplierDto supplierDto)
        {
            if (ModelState.IsValid)
            {
                var supplier = _context.Suppliers.FirstOrDefault(s => s.idSupplier== supplierDto.idSupplier);
                if (supplier == null)
                {
                    return NotFound();
                }

                supplier.nameSupplier = supplierDto.NameSupplier;
                supplier.Phone = supplierDto.Phone;
                supplier.Address = supplierDto.Address;
                supplier.Email = supplierDto.Email;
                supplier.Image = supplierDto.Image;

                _context.Update(supplier);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplierDto);
        }

        // GET: Supplier/Delete/{id}
        public IActionResult Delete(string id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Supplier/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
