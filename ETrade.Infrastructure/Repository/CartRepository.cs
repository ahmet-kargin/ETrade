using ETrade.Application.Interfaces;
using ETrade.Infrastructure.Connection;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETrade.Infrastructure.Repository;

// CartRepository sınıfı, sepetle ilgili veritabanı işlemlerini gerçekleştiren bir repository sınıfıdır.
public class CartRepository : ICartRepository
{
    // ApplicationDbContext, veritabanı işlemlerini gerçekleştirmek için kullanılan bir Entity Framework DbContext sınıfıdır.
    private readonly ApplicationDbContext _context;

    // Constructor, dependency injection kullanarak ApplicationDbContext nesnesini alır.
    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Kullanıcının sepetine ürün ekleyen metod
    public async Task AddToCartAsync(int userId, int productId, int quantity)
    {
        // Kullanıcının sepetini ve sepet öğelerini veritabanından getirir
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        // Eğer kullanıcının sepeti yoksa, yeni bir sepet oluştur ve veritabanına ekle
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem>()
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        // Sepetteki ürünleri kontrol eder, ürün zaten varsa miktarı günceller, yoksa yeni bir ürün ekler
        var cartItem = cart.CartItems
            .FirstOrDefault(ci => ci.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
            _context.CartItems.Update(cartItem);
        }
        else
        {
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            };
            _context.CartItems.Add(cartItem);
        }

        // Değişiklikleri veritabanına kaydeder
        await _context.SaveChangesAsync();
    }

    // Kullanıcının sepetindeki ürünleri getiren metod
    public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int userId)
    {
        // Kullanıcının sepet öğelerini ürün bilgileri ile birlikte getirir
        return await _context.CartItems
            .Where(ci => ci.Cart.UserId == userId)
            .Include(ci => ci.Product) // Ürün bilgilerini de dahil et
            .ToListAsync();
    }

    // Kullanıcının sepetindeki toplam tutarı hesaplayan metod
    public async Task<decimal> GetTotalAmountAsync(int userId)
    {
        // Kullanıcının sepet öğelerini ürün bilgileri ile birlikte getirir
        var cartItems = await _context.CartItems
            .Where(ci => ci.Cart.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();

        // Sepetteki ürünlerin fiyatlarını ve miktarlarını çarparak toplam tutarı hesaplar
        return cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
    }

    // Kullanıcının sepetinden bir ürünü kaldıran metod
    public async Task RemoveFromCartAsync(int userId, int productId)
    {
        // Kullanıcının sepetindeki belirtilen ürünü getirir
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);

        // Eğer ürün sepetinde varsa, ürünü sepetten kaldırır ve değişiklikleri kaydeder
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    // Kullanıcının sepetindeki bir ürünün miktarını güncelleyen metod
    public async Task UpdateCartItemQuantityAsync(int userId, int productId, int quantity)
    {
        // Kullanıcının sepetindeki belirtilen ürünü getirir
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);

        // Eğer ürün sepetinde varsa, ürünün miktarını günceller ve değişiklikleri kaydeder
        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}

