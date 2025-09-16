using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShopAnNhien.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
        
        [StringLength(10)]
        public string? Size { get; set; }
        
        [StringLength(50)]
        public string? Color { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        
        // Computed property
        [NotMapped]
        public decimal TotalPrice => Product?.DisplayPrice * Quantity ?? 0;
    }
}
