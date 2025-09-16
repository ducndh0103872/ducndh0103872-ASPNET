using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShopAnNhien.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? ProductSKU { get; set; }
        
        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
        
        [StringLength(10)]
        public string? Size { get; set; }
        
        [StringLength(50)]
        public string? Color { get; set; }
        
        public string? ProductImageUrl { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        
        // Computed property
        [NotMapped]
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
