﻿using System.ComponentModel.DataAnnotations;

namespace WebThuCung.Dto
{
    public class DetailOrderDto
    {
        public string idOrder { get; set; }

        public string idProduct { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0..")]

        public decimal? totalPrice { get; set; }

        // Để hiển thị tên sản phẩm trong view
     
    }
}
