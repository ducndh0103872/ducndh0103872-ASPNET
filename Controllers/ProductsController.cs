using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShopAnNhien.Data;
using ShoeShopAnNhien.Models;

namespace ShoeShopAnNhien.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            ViewData["Title"] = "Sản Phẩm";
            
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            // Apply filters
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
                ViewBag.Search = search;
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
                ViewBag.CategoryId = categoryId.Value;
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
                ViewBag.BrandId = brandId.Value;
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.DisplayPrice >= minPrice.Value);
                ViewBag.MinPrice = minPrice.Value;
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.DisplayPrice <= maxPrice.Value);
                ViewBag.MaxPrice = maxPrice.Value;
            }

            // Get filter data
            ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
            ViewBag.Brands = await _context.Brands.Where(b => b.IsActive).ToListAsync();

            // Pagination
            const int pageSize = 12;
            var totalItems = await query.CountAsync();
            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["Title"] = product.Name;
            return View(product);
        }

        public async Task<IActionResult> Category(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewData["Title"] = category.Name;
            return RedirectToAction("Index", new { categoryId = id });
        }
    }
}
