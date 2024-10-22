using System.ComponentModel.DataAnnotations;

namespace WebThuCung.Dto
{
    public class SizeDto
    {
        public string idSize { get; set; }

        // Mã định danh cho sản phẩm (Product)
        public string idProduct { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Size cannot be negative.")]
        public int nameSize { get; set; }
    }
}
