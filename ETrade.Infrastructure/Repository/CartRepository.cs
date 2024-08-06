using ETrade.Application.Interfaces;
using ETrade.Infrastructure.Connection;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETrade.Infrastructure.Repository;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddToCartAsync(int userId, int productId, int quantity)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            // Eğer kullanıcının sepeti yoksa yeni bir sepet oluştur
            cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem>()
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var cartItem = cart.CartItems
            .FirstOrDefault(ci => ci.ProductId == productId);

        if (cartItem != null)
        {
            // Eğer ürün zaten sepette varsa, miktarı güncelle
            cartItem.Quantity += quantity;
            _context.CartItems.Update(cartItem);
        }
        else
        {
            // Yeni ürün ekle
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
    }



    public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int userId)
    {
        return await _context.CartItems
            .Where(ci => ci.Cart.UserId == userId)
            .Include(ci => ci.Product) // Ürün bilgilerini de dahil et
            .ToListAsync();
    }


    public async Task<decimal> GetTotalAmountAsync(int userId)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.Cart.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();

        return cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
    }


    public async Task RemoveFromCartAsync(int userId, int productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);

        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }


    public async Task UpdateCartItemQuantityAsync(int userId, int productId, int quantity)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }
    }

}
