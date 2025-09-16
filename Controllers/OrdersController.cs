using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShopAnNhien.Data;
using ShoeShopAnNhien.Models;
using System.Security.Claims;

namespace ShoeShopAnNhien.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MyOrders(int page = 1)
        {
            ViewData["Title"] = "Đơn Hàng Của Tôi";
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            const int pageSize = 10;
            var query = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt);

            var totalItems = await query.CountAsync();
            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            ViewData["Title"] = "Chi Tiết Đơn Hàng";
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
            }

            if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
            {
                return Json(new { success = false, message = "Không thể hủy đơn hàng này" });
            }

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã hủy đơn hàng thành công" });
        }
    }
}
