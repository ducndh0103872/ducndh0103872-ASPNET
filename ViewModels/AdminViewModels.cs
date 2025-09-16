using ShoeShopAnNhien.Models;

namespace ShoeShopAnNhien.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<Order> RecentOrders { get; set; } = new List<Order>();
        public List<Product> TopProducts { get; set; } = new List<Product>();
        public List<Product> LowStockProducts { get; set; } = new List<Product>();
    }

    public class AdminStatisticsViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int TotalOrdersThisMonth { get; set; }
        public int NewCustomersThisMonth { get; set; }
        public List<MonthlyRevenueData> MonthlyRevenueChart { get; set; } = new List<MonthlyRevenueData>();
        public List<CategorySalesData> CategorySalesChart { get; set; } = new List<CategorySalesData>();
    }

    public class MonthlyRevenueData
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class CategorySalesData
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int ProductCount { get; set; }
    }
}
