using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using MyShop.Models.ViewModels;

namespace MyShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ================= DASHBOARD =================
        public async Task<IActionResult> Index()
        {
            var totalRevenue = await _context.Orders
                .Where(o => o.PaymentStatus == "Paid" || o.OrderStatus == "Delivered")
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var totalOrders = await _context.Orders.CountAsync();
            var pendingOrders = await _context.Orders.CountAsync(o => o.OrderStatus == "Pending");
            var totalProducts = await _context.Products.CountAsync();
            var totalArticles = await _context.Articles.CountAsync();

            var recentOrders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt)
                .Take(8)
                .ToListAsync();

            var topProducts = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync();

            var dashboard = new AdminDashboardViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                TotalProducts = totalProducts,
                TotalArticles = totalArticles,
                RecentOrders = recentOrders,
                TopProducts = topProducts
            };

            return View(dashboard);
        }

        // ================= QUẢN LÝ SẢN PHẨM =================
        public async Task<IActionResult> Products(string? search, int? categoryId)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", categoryId);
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.CategoryId = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name");
            return View(new Product { ImageUrl = "/images/products/so-mi/somi_01.jpg" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile? imageUpload)
        {
            if (ModelState.IsValid)
            {
                if (imageUpload != null && imageUpload.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageUpload.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageUpload.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = "/images/uploads/" + uniqueFileName;
                }

                if (string.IsNullOrWhiteSpace(product.Slug))
                {
                    product.Slug = GenerateSlug(product.Name);
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm sản phẩm mới thành công!";
                return RedirectToAction(nameof(Products));
            }

            ViewBag.CategoryId = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.CategoryId = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product, IFormFile? imageUpload)
        {
            if (ModelState.IsValid)
            {
                if (imageUpload != null && imageUpload.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageUpload.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageUpload.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = "/images/uploads/" + uniqueFileName;
                }

                if (string.IsNullOrWhiteSpace(product.Slug))
                {
                    product.Slug = GenerateSlug(product.Name);
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Products));
            }

            ViewBag.CategoryId = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa sản phẩm thành công!";
            }
            return RedirectToAction(nameof(Products));
        }

        // ================= QUẢN LÝ DANH MỤC =================
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(category.Slug))
                {
                    category.Slug = GenerateSlug(category.Name);
                }
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm danh mục mới thành công!";
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(category.Slug))
                {
                    category.Slug = GenerateSlug(category.Name);
                }
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                if (category.Products.Any())
                {
                    TempData["ErrorMessage"] = "Không thể xóa danh mục đang có sản phẩm liên kết!";
                    return RedirectToAction(nameof(Categories));
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa danh mục thành công!";
            }
            return RedirectToAction(nameof(Categories));
        }

        // ================= QUẢN LÝ ĐƠN HÀNG =================
        public async Task<IActionResult> Orders(string? status, string? paymentStatus)
        {
            var query = _context.Orders.Include(o => o.OrderItems).AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(o => o.OrderStatus == status);
            }

            if (!string.IsNullOrWhiteSpace(paymentStatus))
            {
                query = query.Where(o => o.PaymentStatus == paymentStatus);
            }

            var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentPaymentStatus = paymentStatus;
            return View(orders);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int id, string orderStatus, string paymentStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatus = orderStatus;
                order.PaymentStatus = paymentStatus;
                if (paymentStatus == "Paid" && order.PaidAt == null)
                {
                    order.PaidAt = DateTime.Now;
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã cập nhật trạng thái đơn hàng {order.OrderCode}!";
            }
            return RedirectToAction(nameof(OrderDetail), new { id });
        }

        // ================= QUẢN LÝ BÀI VIẾT =================
        public async Task<IActionResult> Articles()
        {
            var articles = await _context.Articles.OrderByDescending(a => a.PublishedAt).ToListAsync();
            return View(articles);
        }

        public IActionResult CreateArticle()
        {
            return View(new Article { ThumbnailUrl = "/images/products/so-mi/somi_01.jpg" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArticle(Article article, IFormFile? thumbnailUpload)
        {
            if (ModelState.IsValid)
            {
                if (thumbnailUpload != null && thumbnailUpload.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(thumbnailUpload.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnailUpload.CopyToAsync(fileStream);
                    }
                    article.ThumbnailUrl = "/images/uploads/" + uniqueFileName;
                }

                if (string.IsNullOrWhiteSpace(article.Slug))
                {
                    article.Slug = GenerateSlug(article.Title);
                }

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đăng bài viết mới thành công!";
                return RedirectToAction(nameof(Articles));
            }
            return View(article);
        }

        public async Task<IActionResult> EditArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticle(Article article, IFormFile? thumbnailUpload)
        {
            if (ModelState.IsValid)
            {
                if (thumbnailUpload != null && thumbnailUpload.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(thumbnailUpload.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnailUpload.CopyToAsync(fileStream);
                    }
                    article.ThumbnailUrl = "/images/uploads/" + uniqueFileName;
                }

                if (string.IsNullOrWhiteSpace(article.Slug))
                {
                    article.Slug = GenerateSlug(article.Title);
                }

                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật bài viết thành công!";
                return RedirectToAction(nameof(Articles));
            }
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa bài viết thành công!";
            }
            return RedirectToAction(nameof(Articles));
        }

        private static string GenerateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "";
            string slug = title.ToLower().Trim();
            string[] vietnamese = new[] { "aáàảãạâấầẩẫậăắằẳẵặ", "eéèẻẽẹêếềểễệ", "iíìỉĩị", "oóòỏõọôốồổỗộơớờởỡợ", "uúùủũụưứừửữự", "yýỳỷỹỵ", "dđ" };
            string[] replacements = new[] { "a", "e", "i", "o", "u", "y", "d" };
            for (int i = 0; i < vietnamese.Length; i++)
            {
                foreach (char c in vietnamese[i])
                {
                    slug = slug.Replace(c.ToString(), replacements[i]);
                }
            }
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-").Trim('-');
            return slug;
        }
    }
}
