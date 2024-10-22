using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebThuCung.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public string idProduct { get; set; }

        [Required]
        [MaxLength(200)]
        public string nameProduct { get; set; }

        public decimal? sellPrice { get; set; }

        [ForeignKey("Branch")]
        public string idBranch { get; set; }

        [ForeignKey("Category")]
        public string idCategory { get; set; }

        [ForeignKey("Color")]
        public string idColor { get; set; }
        [ForeignKey("idPet")]
        // Foreign Key
        public string idPet { get; set; }

        // Navigation Propert

        [Required]
        public int Quantity { get; set; }

        [MaxLength(100)]
        public string Image { get; set; }

        [Required]
        public string Description { get; set; }


        public Branch Branch { get; set; }
        public Category Category { get; set; }
        public Color Color { get; set; }
        public Pet Pet { get; set; }

        public ICollection<DetailVoteWarehouse> DetailVoteWarehouses { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        public ICollection<ImageProduct> ImageProducts { get; set; }
        public ICollection<Size> Sizes { get; set; }
        public ICollection<DetailOrder> DetailOrders { get; set; }
    }
}
