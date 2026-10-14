using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [StringLength(250)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;

        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Chất liệu")]
        public string Material { get; set; } = "Vải cao cấp";

        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        [Range(0, 100000000, ErrorMessage = "Giá bán không hợp lệ")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá bán (VNĐ)")]
        public decimal Price { get; set; }

        [Range(0, 100000000, ErrorMessage = "Giá gốc không hợp lệ")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá gốc niêm yết (VNĐ)")]
        public decimal? OriginalPrice { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp ảnh sản phẩm")]
        [StringLength(500)]
        [Display(Name = "Ảnh đại diện")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Các ảnh phụ")]
        public string? MoreImages { get; set; }

        [StringLength(100)]
        [Display(Name = "Kích thước (cách nhau bởi dấu phẩy)")]
        public string Sizes { get; set; } = "M, L, XL, 2XL, 3XL";

        [StringLength(100)]
        [Display(Name = "Màu sắc")]
        public string Colors { get; set; } = "Trắng, Đen, Xanh";

        [Range(0, 100000)]
        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; } = 50;

        [Display(Name = "Sản phẩm nổi bật")]
        public bool IsFeatured { get; set; } = false;

        [Display(Name = "Kích hoạt hiển thị")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public decimal DiscountPercent
        {
            get
            {
                if (OriginalPrice.HasValue && OriginalPrice.Value > Price && OriginalPrice.Value > 0)
                {
                    return Math.Round((OriginalPrice.Value - Price) / OriginalPrice.Value * 100);
                }
                return 0;
            }
        }
    }
}
