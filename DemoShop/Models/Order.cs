using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Mã đơn hàng")]
        public string OrderCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(150)]
        [Display(Name = "Họ tên người nhận")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại nhận hàng")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(30)]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string? CustomerEmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [StringLength(300)]
        [Display(Name = "Địa chỉ nhận hàng")]
        public string ShippingAddress { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Tỉnh / Thành phố")]
        public string City { get; set; } = "Hà Nội";

        [StringLength(500)]
        [Display(Name = "Ghi chú đơn hàng")]
        public string? Notes { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền hàng")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Phí giao hàng")]
        public decimal ShippingFee { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng thanh toán")]
        public decimal TotalAmount { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Hình thức thanh toán")]
        public string PaymentMethod { get; set; } = "VietQR"; // "VietQR" hoặc "COD"

        [Required, StringLength(50)]
        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus { get; set; } = "Pending"; // "Pending", "Paid", "Refunded"

        [Required, StringLength(50)]
        [Display(Name = "Trạng thái đơn hàng")]
        public string OrderStatus { get; set; } = "Pending"; // "Pending", "Processing", "Shipping", "Delivered", "Cancelled"

        [Display(Name = "Thời gian đặt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Thời gian thanh toán")]
        public DateTime? PaidAt { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
