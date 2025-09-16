using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShopAnNhien.Data;
using ShoeShopAnNhien.Models;
using ShoeShopAnNhien.ViewModels;
using System.Security.Claims;

namespace ShoeShopAnNhien.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Giỏ Hàng";
            
            var cartItems = await GetCartItemsAsync();
            
            var viewModel = new CartViewModel
            {
                CartItems = cartItems,
                SubTotal = cartItems.Sum(item => item.TotalPrice),
                ShippingFee = 30000, // 30,000 VND shipping fee
                TotalAmount = cartItems.Sum(item => item.TotalPrice) + 30000
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng" });
                }

                var product = await _context.Products.FindAsync(request.ProductId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });
                }

                // Check if item already exists in cart
                var existingItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.UserId == userId && 
                                              ci.ProductId == request.ProductId &&
                                              ci.Size == request.Size &&
                                              ci.Color == request.Color);

                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                    existingItem.UpdatedAt = DateTime.Now;
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        UserId = userId,
                        ProductId = request.ProductId,
                        Quantity = request.Quantity,
                        Size = request.Size,
                        Color = request.Color,
                        CreatedAt = DateTime.Now
                    };

                    _context.CartItems.Add(cartItem);
                }

                await _context.SaveChangesAsync();

                var cartCount = await GetCartCountAsync(userId);
                return Json(new { success = true, cartCount = cartCount });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            try
            {
                var userId = GetCurrentUserId();
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

                if (cartItem == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
                }

                if (quantity <= 0)
                {
                    _context.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                    cartItem.UpdatedAt = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật giỏ hàng" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa sản phẩm" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var userId = GetCurrentUserId();
            var count = string.IsNullOrEmpty(userId) ? 0 : await GetCartCountAsync(userId);
            return Json(new { count = count });
        }

        private async Task<List<CartItem>> GetCartItemsAsync()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return new List<CartItem>();

            return await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Brand)
                .Where(ci => ci.UserId == userId)
                .OrderBy(ci => ci.CreatedAt)
                .ToListAsync();
        }

        private async Task<int> GetCartCountAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Size { get; set; }
        public string? Color { get; set; }
    }
}
