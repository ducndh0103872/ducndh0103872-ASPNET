using ShoeShopAnNhien.Models;

namespace ShoeShopAnNhien.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? CouponCode { get; set; }
    }
}
