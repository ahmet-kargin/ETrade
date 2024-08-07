using ETrade.Application.Interfaces;
using ETrade.WebUI.Models.Basket;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers
{
    // CartController, sepet işlemlerini yönetir(ürün ekleme, silme, görüntüleme).
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartService;

        // Dependency Injection kullanılarak gerekli servislerin alınması
        public CartController(IProductRepository productRepository, ICartRepository cartService)
        {
            _productRepository = productRepository;
            _cartService = cartService;
        }

        // Sepet sayfasını görüntüleyen action method
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                // Eğer ID yoksa veya geçerli bir ID değilse, login sayfasına yönlendir
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcının sepetindeki ürünleri getir
            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);

            // CartItem modelinden CartItemViewModel'e dönüşüm
            var cartItemsViewModel = cartItems.Select(item => new CartItemViewModel
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                ProductDescription = item.Product.Description,
                Price = item.Product.Price,
                Quantity = item.Quantity,
                Subtotal = item.Product.Price * item.Quantity
            });

            // Sepetteki toplam tutarı hesapla
            var totalAmount = cartItemsViewModel.Sum(item => item.Subtotal);

            // ViewBag üzerinden sepet öğelerini ve toplam tutarı view'e gönder
            ViewBag.CartItems = cartItemsViewModel;
            ViewBag.TotalAmount = totalAmount;

            return View();
        }

        // Sepetteki ürün sayısını JSON formatında döndüren action method
        [HttpGet]
        public async Task<IActionResult> GetCartItemCount()
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                // Eğer ID yoksa veya geçerli bir ID değilse, sepet sayısını 0 olarak döndür
                return Json(new { count = 0 });
            }

            // Kullanıcının sepetindeki ürünleri getir
            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);

            // Sepetteki ürün sayısını JSON formatında döndür
            return Json(new { count = cartItems.Count() });
        }

        // Sepete ürün ekleyen action method
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                // Eğer ID yoksa veya geçerli bir ID değilse, login sayfasına yönlendir
                return RedirectToAction("Login", "Account");
            }

            // Ürünü ürün repository'si aracılığıyla al
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                // Ürün bulunamazsa, NotFound sonucu döndür
                return NotFound();
            }

            // Sepete ürünü ekle
            await _cartService.AddToCartAsync(userId, productId, quantity);

            // Sepet sayfasına yönlendir
            return RedirectToAction("Index", "Cart");
        }

        // Sepetten ürün silen action method
        [HttpPost]
        public async Task<IActionResult> DeleteCart(int productId)
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                // Eğer ID yoksa veya geçerli bir ID değilse, login sayfasına yönlendir
                return RedirectToAction("Login", "Account");
            }

            // Sepetten ürünü sil
            await _cartService.RemoveFromCartAsync(userId, productId);

            // Sepet sayfasına yönlendir
            return RedirectToAction("Index", "Cart");
        }


    }
}

