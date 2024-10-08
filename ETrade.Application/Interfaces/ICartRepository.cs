﻿using Framework.ETrade.Domain.Entities;

namespace ETrade.Application.Interfaces;

public interface ICartRepository
{
    
    Task AddToCartAsync(int userId, int productId, int quantity);

    // Sepetten ürün kaldırma
    Task RemoveFromCartAsync(int userId, int productId);

    // Sepet güncelleme
    Task UpdateCartItemQuantityAsync(int userId, int productId, int quantity);

    // Kullanıcının sepetini alma
    Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int userId);

    // Sepet toplamını alma
    Task<decimal> GetTotalAmountAsync(int userId);
}
