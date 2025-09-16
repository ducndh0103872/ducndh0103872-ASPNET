using System.ComponentModel.DataAnnotations;

namespace ShoeShopAnNhien.Models
{
    public class Review
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Đánh giá là bắt buộc")]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int Rating { get; set; }
        
        [StringLength(1000, ErrorMessage = "Bình luận không được vượt quá 1000 ký tự")]
        public string? Comment { get; set; }
        
        [Required(ErrorMessage = "Tên người đánh giá là bắt buộc")]
        [StringLength(100)]
        public string ReviewerName { get; set; } = string.Empty;
        
        public bool IsApproved { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual Product Product { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
