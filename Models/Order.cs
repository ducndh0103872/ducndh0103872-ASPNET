using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShopAnNhien.Models
{
    public enum OrderStatus
    {
        Pending = 0,        // Chờ xử lý
        Confirmed = 1,      // Đã xác nhận
        Processing = 2,     // Đang xử lý
        Shipping = 3,       // Đang giao hàng
        Delivered = 4,      // Đã giao hàng
        Cancelled = 5,      // Đã hủy
        Returned = 6        // Đã trả hàng
    }

    public enum PaymentStatus
    {
        Pending = 0,        // Chờ thanh toán
        Paid = 1,          // Đã thanh toán
        Failed = 2,        // Thanh toán thất bại
        Refunded = 3       // Đã hoàn tiền
    }

    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string OrderNumber { get; set; } = string.Empty;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Họ tên người nhận là bắt buộc")]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(20)]
        public string CustomerPhone { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc")]
        [StringLength(500)]
        public string ShippingAddress { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; } = 0;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; } = 0;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        
        [StringLength(50)]
        public string? PaymentMethod { get; set; }
        
        [StringLength(100)]
        public string? PaymentTransactionId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
