using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề bài viết")]
        [StringLength(250)]
        [Display(Name = "Tiêu đề bài viết")]
        public string Title { get; set; } = string.Empty;

        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tóm tắt ngắn")]
        [StringLength(500)]
        [Display(Name = "Tóm tắt ngắn")]
        public string Summary { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập nội dung bài viết")]
        [Display(Name = "Nội dung bài viết")]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Ảnh đại diện")]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Tác giả")]
        public string Author { get; set; } = "Thời trang Ngọc Hiển";

        [Display(Name = "Ngày đăng")]
        public DateTime PublishedAt { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái hiển thị")]
        public bool IsPublished { get; set; } = true;

        [Display(Name = "Lượt xem")]
        public int ViewCount { get; set; } = 0;
    }
}
