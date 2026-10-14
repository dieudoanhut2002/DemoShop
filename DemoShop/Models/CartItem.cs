namespace MyShop.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public string SelectedSize { get; set; } = "L";
        public string SelectedColor { get; set; } = "Tiêu chuẩn";

        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
