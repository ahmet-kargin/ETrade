using ETrade.Application.Interfaces;
using ETrade.Infrastructure.Repository;
using ETrade.WebUI.Models.Basket;
using ETrade.WebUI.Models.OrderModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers
{
    // CartController, sepet işlemlerini yönetir(ürün ekleme, silme, görüntüleme).
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartService;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;

        // Dependency Injection kullanılarak gerekli servislerin alınması
        public CartController(IProductRepository productRepository, ICartRepository cartService, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _productRepository = productRepository;
            _cartService = cartService;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
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
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
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

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder()
        {
            // Kullanıcının kimliğini session'dan alır.
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                // Kullanıcı kimliği yoksa veya geçersizse Login sayfasına yönlendirir.
                return RedirectToAction("Login", "Account");
            }
            // Kullanıcının sepetindeki ürünleri alır.
            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);
            if (cartItems == null || !cartItems.Any())
            {
                // Sepet boşsa ana sayfaya yönlendirir.
                return RedirectToAction("Index");
            }

            // var totalAmount = cartItems.Sum(item => item.Product.Price * item.Quantity);
            decimal totalAmount = 0;
            foreach (var cartItem in cartItems)
            {
                totalAmount += cartItem.Product.Price * cartItem.Quantity;
            }

            var order = new Framework.ETrade.Domain.Entities.Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                OrderItems = cartItems.Select(cartItem => new Framework.ETrade.Domain.Entities.OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price
                }).ToList()
            };
            // Siparişi veritabanına ekler ve değişiklikleri kaydeder.
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            // Kullanıcının sepetini temizler.
            HttpContext.Session.Remove("CartItems");

            foreach (var cartItem in cartItems)
            {
                await _cartService.RemoveFromCartAsync(userId, cartItem.ProductId);
            }

            // Sipariş listesine yönlendirir.
            return RedirectToAction("OrderList", new { orderId = order.Id });
        }

        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            // Kullanıcının kimliğini session'dan alır.
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                // Kullanıcı kimliği yoksa veya geçersizse Login sayfasına yönlendirir.
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcının siparişlerini alır.
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            // Siparişleri OrderModel'e dönüştürür.
            var orderViewModels = orders.Select(order => new OrderModel
            {
                Id = order.Id,
                UserId = order.UserId.ToString(),
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemModel
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    ProductName = oi.Product.Name 
                }).ToList()
            }).ToList();

            return View(orderViewModels); // View'e gönderme
        }

    }


}


