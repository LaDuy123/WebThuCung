using System.ComponentModel.DataAnnotations;

namespace WebThuCung.Dto
{
    public class SizeDto
    {
        public string idSize { get; set; }

        // Mã định danh cho sản phẩm (Product)
        public string idProduct { get; set; }

    
        public string nameSize { get; set; }
    }
}
