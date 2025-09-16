using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShoeShopAnNhien.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; } = string.Empty;
        
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string? Address { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        public string? Gender { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
