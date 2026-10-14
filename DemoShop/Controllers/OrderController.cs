using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Extensions;
using MyShop.Models;
using MyShop.Models.ViewModels;
using MyShop.Services;

namespace MyShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IVietQrService _vietQrService;
        private const string CartSessionKey = "ShopCartSession";

        public OrderController(ApplicationDbContext context, IVietQrService vietQrService)
        {
            _context = context;
            _vietQrService = vietQrService;
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Vui lòng chọn sản phẩm trước khi thanh toán!";
                return RedirectToAction("Index", "Cart");
            }

            decimal subTotal = cart.Sum(i => i.TotalPrice);
            decimal shippingFee = subTotal >= 500000 ? 0 : 30000;
            decimal totalAmount = subTotal + shippingFee;

            ViewBag.Cart = cart;
            ViewBag.SubTotal = subTotal;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.TotalAmount = totalAmount;

            var model = new CheckoutViewModel
            {
                PaymentMethod = "VietQR",
                City = "Hà Nội"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống. Vui lòng chọn sản phẩm!";
                return RedirectToAction("Index", "Cart");
            }

            decimal subTotal = cart.Sum(i => i.TotalPrice);
            decimal shippingFee = subTotal >= 500000 ? 0 : 30000;
            decimal totalAmount = subTotal + shippingFee;

            if (ModelState.IsValid)
            {
                // Sinh mã đơn hàng độc nhất
                string orderCode = "NH" + DateTime.Now.ToString("yyMMddHHmmss");

                var order = new Order
                {
                    OrderCode = orderCode,
                    CustomerName = model.CustomerName.Trim(),
                    CustomerPhone = model.CustomerPhone.Trim(),
                    CustomerEmail = model.CustomerEmail?.Trim(),
                    ShippingAddress = model.ShippingAddress.Trim(),
                    City = model.City.Trim(),
                    Notes = model.Notes?.Trim(),
                    SubTotal = subTotal,
                    ShippingFee = shippingFee,
                    TotalAmount = totalAmount,
                    PaymentMethod = model.PaymentMethod,
                    PaymentStatus = model.PaymentMethod == "VietQR" ? "Pending" : "Pending",
                    OrderStatus = "Pending",
                    CreatedAt = DateTime.Now
                };

                foreach (var item in cart)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductImage = item.ProductImage,
                        SelectedSize = item.SelectedSize,
                        SelectedColor = item.SelectedColor,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice
                    });
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Làm trống giỏ hàng sau khi đặt thành công
                HttpContext.Session.Remove(CartSessionKey);

                return RedirectToAction(nameof(PaymentSuccess), new { id = order.Id });
            }

            ViewBag.Cart = cart;
            ViewBag.SubTotal = subTotal;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.TotalAmount = totalAmount;

            return View(model);
        }

        public async Task<IActionResult> PaymentSuccess(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Tạo mã QR VietQR nếu phương thức là VietQR
            if (order.PaymentMethod == "VietQR")
            {
                ViewBag.VietQrUrl = _vietQrService.GenerateVietQrImageUrl(order.TotalAmount, order.OrderCode);
                ViewBag.BankId = _vietQrService.BankId;
                ViewBag.BankName = _vietQrService.BankName;
                ViewBag.AccountNo = _vietQrService.AccountNo;
                ViewBag.AccountName = _vietQrService.AccountName;

                // Fallback QR tạo bằng thư viện QRCoder
                string qrPayload = $"STK:{_vietQrService.AccountNo}|BANK:{_vietQrService.BankId}|AMOUNT:{order.TotalAmount}|MEMO:{order.OrderCode}";
                ViewBag.FallbackQrBase64 = _vietQrService.GenerateFallbackQrBase64(qrPayload);
            }

            return View(order);
        }

        public async Task<IActionResult> Tracking(string? orderCode, string? phone)
        {
            if (string.IsNullOrWhiteSpace(orderCode) && string.IsNullOrWhiteSpace(phone))
            {
                return View();
            }

            var query = _context.Orders.Include(o => o.OrderItems).AsQueryable();

            if (!string.IsNullOrWhiteSpace(orderCode))
            {
                query = query.Where(o => o.OrderCode == orderCode.Trim());
            }
            else if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(o => o.CustomerPhone == phone.Trim());
            }

            var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
            return View(orders);
        }
    }
}
