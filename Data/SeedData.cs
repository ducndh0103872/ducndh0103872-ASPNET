using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoeShopAnNhien.Models;

namespace ShoeShopAnNhien.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Tạo roles
            await CreateRoles(roleManager);
            
            // Tạo admin user
            await CreateAdminUser(userManager);
            
            // Tạo categories
            await CreateCategories(context);
            
            // Tạo brands
            await CreateBrands(context);
            
            // Tạo products
            await CreateProducts(context);
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Customer" };
            
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@shoegiayannnhien.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Quản trị viên",
                    EmailConfirmed = true,
                    IsActive = true
                };
                
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private static async Task CreateCategories(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            var categories = new List<Category>
            {
                new Category { Name = "Giày Thể Thao", Description = "Giày thể thao nam nữ đa dạng", ImageUrl = "/images/categories/thethao.jpg" },
                new Category { Name = "Giày Cao Gót", Description = "Giày cao gót thời trang cho nữ", ImageUrl = "/images/categories/caogot.jpg" },
                new Category { Name = "Giày Tây", Description = "Giày tây lịch sự cho nam", ImageUrl = "/images/categories/tay.jpg" },
                new Category { Name = "Giày Sandal", Description = "Sandal thoải mái cho mùa hè", ImageUrl = "/images/categories/sandal.jpg" },
                new Category { Name = "Giày Boot", Description = "Boots phong cách và bền bỉ", ImageUrl = "/images/categories/boot.jpg" }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        private static async Task CreateBrands(ApplicationDbContext context)
        {
            if (await context.Brands.AnyAsync()) return;

            var brands = new List<Brand>
            {
                new Brand { Name = "Nike", Description = "Thương hiệu thể thao hàng đầu thế giới", LogoUrl = "/images/brands/nike.png" },
                new Brand { Name = "Adidas", Description = "Thương hiệu thể thao nổi tiếng", LogoUrl = "/images/brands/adidas.png" },
                new Brand { Name = "Converse", Description = "Giày thể thao phong cách cổ điển", LogoUrl = "/images/brands/converse.png" },
                new Brand { Name = "Vans", Description = "Giày skateboard và lifestyle", LogoUrl = "/images/brands/vans.png" },
                new Brand { Name = "Biti's", Description = "Thương hiệu giày Việt Nam", LogoUrl = "/images/brands/bitis.png" }
            };

            context.Brands.AddRange(brands);
            await context.SaveChangesAsync();
        }

        private static async Task CreateProducts(ApplicationDbContext context)
        {
            if (await context.Products.AnyAsync()) return;

            var categories = await context.Categories.ToListAsync();
            var brands = await context.Brands.ToListAsync();

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Nike Air Max 270",
                    SKU = "NK-AM270-001",
                    Description = "Giày thể thao Nike Air Max 270 với công nghệ đệm khí tiên tiến, mang lại sự thoải mái tối đa cho người sử dụng. Thiết kế hiện đại, phù hợp cho cả nam và nữ.",
                    Price = 2500000,
                    SalePrice = 2200000,
                    StockQuantity = 50,
                    MainImageUrl = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/99486859-0ff3-46b4-949b-2d16af2ad421/custom-nike-air-max-90.png",
                    ImageUrls = "['/images/products/nike-air-max-270-1.jpg','/images/products/nike-air-max-270-2.jpg','/images/products/nike-air-max-270-3.jpg']",
                    AvailableSizes = "['36','37','38','39','40','41','42','43']",
                    AvailableColors = "['Đen','Trắng','Xanh Navy']",
                    IsFeatured = true,
                    CategoryId = categories.First(c => c.Name == "Giày Thể Thao").Id,
                    BrandId = brands.First(b => b.Name == "Nike").Id
                },
                new Product
                {
                    Name = "Adidas Ultraboost 22",
                    SKU = "AD-UB22-002",
                    Description = "Giày chạy bộ Adidas Ultraboost 22 với công nghệ Boost độc quyền, cung cấp năng lượng trả về tối ưu. Phù hợp cho việc tập luyện và chạy bộ hàng ngày.",
                    Price = 3200000,
                    StockQuantity = 30,
                    MainImageUrl = "https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/fbaf991a78bc4896a3e9ad7800abcec6_9366/Ultraboost_22_Shoes_Black_GZ0127_01_standard.jpg",
                    ImageUrls = "['/images/products/adidas-ultraboost-22-1.jpg','/images/products/adidas-ultraboost-22-2.jpg']",
                    AvailableSizes = "['36','37','38','39','40','41','42','43','44']",
                    AvailableColors = "['Đen','Trắng','Xám']",
                    IsFeatured = true,
                    CategoryId = categories.First(c => c.Name == "Giày Thể Thao").Id,
                    BrandId = brands.First(b => b.Name == "Adidas").Id
                },
                new Product
                {
                    Name = "Converse Chuck Taylor All Star",
                    SKU = "CV-CT-003",
                    Description = "Giày Converse Chuck Taylor All Star cổ điển, biểu tượng của phong cách street style. Thiết kế vượt thời gian, phù hợp với mọi lứa tuổi.",
                    Price = 1200000,
                    SalePrice = 990000,
                    StockQuantity = 80,
                    MainImageUrl = "https://www.converse.com/dw/image/v2/BCZC_PRD/on/demandware.static/-/Sites-cnv-master-catalog/default/dw2f8b0b3e/images/a_107/M9160_A_107X1.jpg",
                    ImageUrls = "['/images/products/converse-chuck-taylor-1.jpg','/images/products/converse-chuck-taylor-2.jpg']",
                    AvailableSizes = "['35','36','37','38','39','40','41','42']",
                    AvailableColors = "['Đen','Trắng','Đỏ','Xanh']",
                    IsFeatured = false,
                    CategoryId = categories.First(c => c.Name == "Giày Thể Thao").Id,
                    BrandId = brands.First(b => b.Name == "Converse").Id
                },
                new Product
                {
                    Name = "Giày Cao Gót Mũi Nhọn",
                    SKU = "CG-MN-004",
                    Description = "Giày cao gót mũi nhọn thanh lịch, phù hợp cho các dịp trang trọng và công sở. Chất liệu da cao cấp, đế cao 7cm tạo dáng thon gọn.",
                    Price = 1800000,
                    StockQuantity = 25,
                    MainImageUrl = "https://product.hstatic.net/1000230642/product/giay-cao-got-nu-dep-2021-cg001_grande.jpg",
                    ImageUrls = "['/images/products/cao-got-mui-nhon-1.jpg','/images/products/cao-got-mui-nhon-2.jpg']",
                    AvailableSizes = "['35','36','37','38','39','40']",
                    AvailableColors = "['Đen','Nude','Đỏ']",
                    IsFeatured = false,
                    CategoryId = categories.First(c => c.Name == "Giày Cao Gót").Id,
                    BrandId = brands.First().Id
                },
                new Product
                {
                    Name = "Giày Tây Oxford",
                    SKU = "GT-OX-005",
                    Description = "Giày tây Oxford cổ điển cho nam giới, phù hợp cho môi trường công sở và các sự kiện trang trọng. Chất liệu da thật cao cấp.",
                    Price = 2200000,
                    SalePrice = 1980000,
                    StockQuantity = 40,
                    MainImageUrl = "https://product.hstatic.net/200000265619/product/giay-tay-nam-oxford-da-bo-that-cao-cap-od15-1_grande.jpg",
                    ImageUrls = "['/images/products/giay-tay-oxford-1.jpg','/images/products/giay-tay-oxford-2.jpg']",
                    AvailableSizes = "['38','39','40','41','42','43','44']",
                    AvailableColors = "['Đen','Nâu','Nâu Đậm']",
                    IsFeatured = true,
                    CategoryId = categories.First(c => c.Name == "Giày Tây").Id,
                    BrandId = brands.First().Id
                },
                new Product
                {
                    Name = "Vans Old Skool",
                    SKU = "VN-OS-006",
                    Description = "Giày Vans Old Skool với thiết kế sọc đặc trưng, phong cách skateboard cổ điển. Chất liệu canvas và da lộn bền bỉ.",
                    Price = 1500000,
                    SalePrice = 1350000,
                    StockQuantity = 60,
                    MainImageUrl = "https://images.vans.com/is/image/Vans/D3HY28-HERO?$583x583$",
                    ImageUrls = "['/images/products/vans-old-skool-1.jpg','/images/products/vans-old-skool-2.jpg']",
                    AvailableSizes = "['36','37','38','39','40','41','42','43']",
                    AvailableColors = "['Đen Trắng','Xanh Navy','Đỏ Trắng']",
                    IsFeatured = true,
                    CategoryId = categories.First(c => c.Name == "Giày Thể Thao").Id,
                    BrandId = brands.First(b => b.Name == "Vans").Id
                },
                new Product
                {
                    Name = "Sandal Nữ Đế Xuồng",
                    SKU = "SD-DX-007",
                    Description = "Sandal nữ đế xuồng thời trang, thoải mái cho mùa hè. Quai ngang điều chỉnh được, phù hợp với nhiều dáng chân.",
                    Price = 800000,
                    StockQuantity = 45,
                    MainImageUrl = "https://product.hstatic.net/1000230642/product/sandal-nu-dep-2021-sd001_grande.jpg",
                    ImageUrls = "['/images/products/sandal-de-xuong-1.jpg','/images/products/sandal-de-xuong-2.jpg']",
                    AvailableSizes = "['35','36','37','38','39','40']",
                    AvailableColors = "['Nâu','Đen','Kem']",
                    IsFeatured = false,
                    CategoryId = categories.First(c => c.Name == "Giày Sandal").Id,
                    BrandId = brands.First().Id
                },
                new Product
                {
                    Name = "Boot Da Nam",
                    SKU = "BT-DN-008",
                    Description = "Boot da nam phong cách cổ điển, chất liệu da thật cao cấp. Thiết kế mạnh mẽ, phù hợp cho thời tiết lạnh.",
                    Price = 2800000,
                    StockQuantity = 20,
                    MainImageUrl = "https://product.hstatic.net/1000230642/product/boot-nam-da-that-bt001_grande.jpg",
                    ImageUrls = "['/images/products/boot-da-nam-1.jpg','/images/products/boot-da-nam-2.jpg']",
                    AvailableSizes = "['39','40','41','42','43','44']",
                    AvailableColors = "['Nâu','Đen','Nâu Đậm']",
                    IsFeatured = false,
                    CategoryId = categories.First(c => c.Name == "Giày Boot").Id,
                    BrandId = brands.First().Id
                },
                new Product
                {
                    Name = "Biti's Hunter Street",
                    SKU = "BT-HS-009",
                    Description = "Giày thể thao Biti's Hunter Street - sản phẩm Việt Nam chất lượng cao. Thiết kế trẻ trung, năng động.",
                    Price = 650000,
                    SalePrice = 520000,
                    StockQuantity = 100,
                    MainImageUrl = "https://product.hstatic.net/1000230642/product/dsmh00700den__6__beaeb93d4e8e4b9e9b7c0c7c5c5c5c5c.jpg",
                    ImageUrls = "['/images/products/bitis-hunter-street-1.jpg','/images/products/bitis-hunter-street-2.jpg']",
                    AvailableSizes = "['36','37','38','39','40','41','42','43']",
                    AvailableColors = "['Đen','Trắng','Xanh','Đỏ']",
                    IsFeatured = true,
                    CategoryId = categories.First(c => c.Name == "Giày Thể Thao").Id,
                    BrandId = brands.First(b => b.Name == "Biti's").Id
                },
                new Product
                {
                    Name = "Giày Cao Gót Đế Bệt",
                    SKU = "CG-DB-010",
                    Description = "Giày cao gót đế bệt thanh lịch, phù hợp cho công sở và dạo phố. Thiết kế tối giản, dễ phối đồ.",
                    Price = 1200000,
                    SalePrice = 960000,
                    StockQuantity = 35,
                    MainImageUrl = "https://product.hstatic.net/1000230642/product/giay-chay-bo-nam-pr001_grande.jpg",
                    ImageUrls = "['/images/products/cao-got-de-bet-1.jpg','/images/products/cao-got-de-bet-2.jpg']",
                    AvailableSizes = "['35','36','37','38','39','40']",
                    AvailableColors = "['Đen','Nude','Xám']",
                    IsFeatured = false,
                    CategoryId = categories.First(c => c.Name == "Giày Cao Gót").Id,
                    BrandId = brands.First().Id
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
