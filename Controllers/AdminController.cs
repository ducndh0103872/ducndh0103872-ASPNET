using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShopAnNhien.Data;
using ShoeShopAnNhien.Models;
using ShoeShopAnNhien.ViewModels;

namespace ShoeShopAnNhien.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Quản Trị Viên - Dashboard";

            var viewModel = new AdminDashboardViewModel
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalCustomers = await _context.Users.CountAsync(u => u.IsActive),
                TotalRevenue = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .SumAsync(o => o.TotalAmount),
                
                RecentOrders = await _context.Orders
                    .Include(o => o.User)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                
                TopProducts = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Where(p => p.IsActive)
                    .OrderByDescending(p => p.IsFeatured)
                    .ThenBy(p => p.Name)
                    .Take(5)
                    .ToListAsync(),
                
                LowStockProducts = await _context.Products
                    .Include(p => p.Brand)
                    .Where(p => p.IsActive && p.StockQuantity <= 10)
                    .OrderBy(p => p.StockQuantity)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // Products Management
        public async Task<IActionResult> Products(int page = 1)
        {
            ViewData["Title"] = "Quản Lý Sản Phẩm";
            
            const int pageSize = 10;
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt);

            var totalItems = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;

            return View(products);
        }

        // Orders Management
        public async Task<IActionResult> Orders(int page = 1)
        {
            ViewData["Title"] = "Quản Lý Đơn Hàng";
            
            const int pageSize = 10;
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
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

        // Customers Management
        public async Task<IActionResult> Customers(int page = 1)
        {
            ViewData["Title"] = "Quản Lý Khách Hàng";
            
            const int pageSize = 10;
            var query = _context.Users
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.CreatedAt);

            var totalItems = await query.CountAsync();
            var customers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;

            return View(customers);
        }

        // Categories Management
        public async Task<IActionResult> Categories()
        {
            ViewData["Title"] = "Quản Lý Danh Mục";
            
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        // Brands Management
        public async Task<IActionResult> Brands()
        {
            ViewData["Title"] = "Quản Lý Thương Hiệu";
            
            var brands = await _context.Brands
                .OrderBy(b => b.Name)
                .ToListAsync();

            return View(brands);
        }

        // Statistics
        public async Task<IActionResult> Statistics()
        {
            ViewData["Title"] = "Thống Kê Doanh Thu";
            
            var viewModel = new AdminStatisticsViewModel
            {
                TotalRevenue = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .SumAsync(o => o.TotalAmount),
                
                MonthlyRevenue = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Delivered && 
                               o.CreatedAt.Month == DateTime.Now.Month &&
                               o.CreatedAt.Year == DateTime.Now.Year)
                    .SumAsync(o => o.TotalAmount),
                
                TotalOrdersThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.Month == DateTime.Now.Month &&
                               o.CreatedAt.Year == DateTime.Now.Year)
                    .CountAsync(),
                
                NewCustomersThisMonth = await _context.Users
                    .Where(u => u.CreatedAt.Month == DateTime.Now.Month &&
                               u.CreatedAt.Year == DateTime.Now.Year)
                    .CountAsync()
            };

            return View(viewModel);
        }

        // Update Order Status
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                order.Status = status;
                order.UpdatedAt = DateTime.Now;

                if (status == OrderStatus.Delivered)
                {
                    order.DeliveredAt = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật" });
            }
        }

        // Toggle Product Status
        [HttpPost]
        public async Task<IActionResult> ToggleProductStatus(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                product.IsActive = !product.IsActive;
                product.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật" });
            }
        }
    }
}
