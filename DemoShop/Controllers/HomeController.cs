using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using MyShop.Models.ViewModels;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            var featuredProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.IsFeatured)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync();

            var shirtProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Category != null && p.Category.Slug == "ao-so-mi-nam")
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync();

            var winterProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Category != null && p.Category.Slug == "ao-ret-mua-dong")
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync();

            var latestArticles = await _context.Articles
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.PublishedAt)
                .Take(3)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.FeaturedProducts = featuredProducts;
            ViewBag.ShirtProducts = shirtProducts;
            ViewBag.WinterProducts = winterProducts;
            ViewBag.LatestArticles = latestArticles;

            return View();
        }

        public async Task<IActionResult> Products(int? categoryId, string? search, string? size, string? sort, decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            int pageSize = 12;
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term) || p.Description.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(size))
            {
                query = query.Where(p => p.Sizes.Contains(size));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = sort switch
            {
                "price-asc" => query.OrderBy(p => p.Price),
                "price-desc" => query.OrderByDescending(p => p.Price),
                "name-asc" => query.OrderBy(p => p.Name),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.DisplayOrder).ToListAsync();

            var viewModel = new ShopFilterViewModel
            {
                Products = products,
                Categories = categories,
                CategoryId = categoryId,
                Search = search,
                Size = size,
                Sort = sort,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
            {
                return NotFound();
            }

            var relatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && p.IsActive)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;
            return View(product);
        }

        public async Task<IActionResult> Articles(int page = 1)
        {
            int pageSize = 6;
            var query = _context.Articles.Where(a => a.IsPublished).OrderByDescending(a => a.PublishedAt);
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var articles = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(articles);
        }

        public async Task<IActionResult> ArticleDetail(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id && a.IsPublished);
            if (article == null)
            {
                return NotFound();
            }

            article.ViewCount++;
            await _context.SaveChangesAsync();

            var recentArticles = await _context.Articles
                .Where(a => a.Id != id && a.IsPublished)
                .OrderByDescending(a => a.PublishedAt)
                .Take(4)
                .ToListAsync();

            ViewBag.RecentArticles = recentArticles;
            return View(article);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
