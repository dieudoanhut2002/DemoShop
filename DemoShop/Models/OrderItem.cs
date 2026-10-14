using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [Required, StringLength(250)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Ảnh")]
        public string ProductImage { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Kích thước")]
        public string SelectedSize { get; set; } = "L";

        [StringLength(50)]
        [Display(Name = "Màu sắc")]
        public string SelectedColor { get; set; } = "Tiêu chuẩn";

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Thành tiền")]
        public decimal TotalPrice { get; set; }
    }
}
