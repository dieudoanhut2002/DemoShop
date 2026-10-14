using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Extensions;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "ShopCartSession";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            decimal subTotal = cart.Sum(i => i.TotalPrice);
            decimal shippingFee = (subTotal >= 500000 || subTotal == 0) ? 0 : 30000;
            decimal totalAmount = subTotal + shippingFee;

            ViewBag.SubTotal = subTotal;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.TotalAmount = totalAmount;

            return View(cart);
        }

        [HttpPost]
        public IActionResult Add(int productId, int quantity = 1, string size = "L", string color = "Tiêu chuẩn")
        {
            var product = _context.Products.Find(productId);
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            if (quantity <= 0) quantity = 1;

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == productId && i.SelectedSize == size && i.SelectedColor == color);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImage = product.ImageUrl,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    SelectedSize = size,
                    SelectedColor = color
                });
            }

            SaveCart(cart);

            TempData["SuccessMessage"] = $"Đã thêm \"{product.Name}\" vào giỏ hàng thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string size, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId && i.SelectedSize == size);

            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    cart.Remove(item);
                }
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId, string size)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId && i.SelectedSize == size);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng!";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartSessionKey);
            TempData["SuccessMessage"] = "Đã làm trống giỏ hàng!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            int count = cart.Sum(i => i.Quantity);
            return Json(new { count });
        }
    }
}
