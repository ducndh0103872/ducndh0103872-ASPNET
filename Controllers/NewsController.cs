using Microsoft.AspNetCore.Mvc;

namespace ShoeShopAnNhien.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Tin Tức";
            return View();
        }

        public IActionResult Details(int id)
        {
            ViewData["Title"] = "Chi Tiết Tin Tức";
            ViewBag.NewsId = id;
            return View();
        }
    }
}
