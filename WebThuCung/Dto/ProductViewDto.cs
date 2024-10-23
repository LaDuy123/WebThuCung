namespace WebThuCung.Dto
{
    public class ProductViewDto
    {
        public string idProduct { get; set; }
        public string nameProduct { get; set; }
        public decimal? sellPrice { get; set; }
        public string Image { get; set; }
        public string idBranch { get; set; }
        public string idCategori { get; set; }
        public string nameBranch { get; set; }
        public string nameCategory { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string idColor { get; set; }
        public string nameColor { get; set; }
        public List<string> Sizes { get; set; }

        public string Logo { get; set; }
        public List<int> Discounts { get; set; }

    }
}
