using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShopAnNhien.Data;
using ShoeShopAnNhien.Models;
using ShoeShopAnNhien.ViewModels;

namespace ShoeShopAnNhien.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            Categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync(),

            FeaturedProducts = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.IsFeatured)
                .OrderBy(p => p.Name)
                .Take(8)
                .ToListAsync()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewData["Title"] = "Giới Thiệu";
        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Title"] = "Liên Hệ";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
