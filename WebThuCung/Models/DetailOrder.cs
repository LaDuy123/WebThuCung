using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebThuCung.Models
{
    [Table("DetailOrder")]
    [PrimaryKey("idOrder", "idProduct")]
    public class DetailOrder
    {
        [Key, Column(Order = 0)]
        public string idOrder { get; set; }

        [Key, Column(Order = 1)]
        public string idProduct { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal? totalPrice { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
        public decimal CalculateTotalPrice()
        {
            // Kiểm tra xem Product có khác null không trước khi truy cập Price
            if (Product != null)
            {
                return Quantity * Product.sellPrice; // Sử dụng giá từ Product
            }

            return 0; // Nếu Product là null, trả về 0
        }
    }

}
