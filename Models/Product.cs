using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShopAnNhien.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
        public string? SKU { get; set; }
        
        [Required(ErrorMessage = "Mô tả sản phẩm là bắt buộc")]
        [StringLength(2000, ErrorMessage = "Mô tả không được vượt quá 2000 ký tự")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi phải lớn hơn hoặc bằng 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }
        
        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int StockQuantity { get; set; }
        
        public string? MainImageUrl { get; set; }
        
        public string? ImageUrls { get; set; } // JSON string chứa danh sách URL hình ảnh
        
        public string? AvailableSizes { get; set; } // JSON string chứa danh sách size
        
        public string? AvailableColors { get; set; } // JSON string chứa danh sách màu sắc
        
        public bool IsFeatured { get; set; } = false;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign Keys
        [Required(ErrorMessage = "Danh mục sản phẩm là bắt buộc")]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "Thương hiệu sản phẩm là bắt buộc")]
        public int BrandId { get; set; }
        
        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        
        // Computed properties
        [NotMapped]
        public decimal DisplayPrice => SalePrice ?? Price;
        
        [NotMapped]
        public bool IsOnSale => SalePrice.HasValue && SalePrice < Price;
        
        [NotMapped]
        public decimal DiscountPercentage => IsOnSale ? Math.Round(((Price - SalePrice!.Value) / Price) * 100, 0) : 0;
        
        [NotMapped]
        public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;
        
        [NotMapped]
        public int ReviewCount => Reviews.Count;
    }
}
