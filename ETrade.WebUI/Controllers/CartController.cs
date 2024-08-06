using ETrade.Application.Interfaces;
using ETrade.Domain.Entities;
using ETrade.WebUI.Models.Basket;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ETrade.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartService; // Bu servisi eklemeniz gerekebilir

        public CartController(IProductRepository productRepository, ICartRepository cartService)
        {
            _productRepository = productRepository;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);

            // CartItem modelinden CartItemViewModel'e dönüşüm
            var cartItemsViewModel = cartItems.Select(item => new CartItemViewModel
            {
                ProductName = item.Product.Name,
                ProductDescription = item.Product.Description,
                Price = item.Product.Price,
                Quantity = item.Quantity,
                Subtotal = item.Product.Price * item.Quantity
            });

            var totalAmount = cartItemsViewModel.Sum(item => item.Subtotal);

            ViewBag.CartItems = cartItemsViewModel;
            ViewBag.TotalAmount = totalAmount;

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetCartItemCount()
        {
            
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Json(new { count = 0 });
            }

            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);
            return Json(new { count = cartItems.Count() });
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Ürünü ürün repository'si aracılığıyla al
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Sepete ürünü ekle
            await _cartService.AddToCartAsync(userId, productId, quantity);

            // Sepet sayfasına yönlendir
            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFromCartAsync(int productId)
        {
            var userIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            await _cartService.RemoveFromCartAsync(userId, productId);
            return RedirectToAction("Index", "Cart");
        }


    }
}

