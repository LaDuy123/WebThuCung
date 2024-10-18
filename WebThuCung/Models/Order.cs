using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebThuCung.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public string idOrder { get; set; }

        [ForeignKey("Customer")]
        public string idCustomer { get; set; }

        [Required]
        public DateTime dateFrom { get; set; }

        public DateTime? dateTo { get; set; }

        public bool? statusOrder { get; set; }

        public bool? statusPay { get; set; }

        public decimal? totalOrder { get; set; }

        // Navigation Properties
        public Customer Cusstomer { get; set; }
        public ICollection<DetailOrder> DetailOrders { get; set; }
    }
}
