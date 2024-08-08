using ETrade.Application.Interfaces;
using ETrade.Infrastructure.Connection;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETrade.Infrastructure.Repository;


public class OrderItemRepository : IOrderItemRepository
{
    private readonly ApplicationDbContext _context;

    // Constructor: ApplicationDbContext'i dependency injection ile alır.
    public OrderItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    // Yeni bir order item ekler.
    public async Task AddAsync(OrderItem orderItem)
    {
        await _context.OrderItems.AddAsync(orderItem);
    }
    // Veritabanına yapılan değişiklikleri kaydeder.
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // Belirtilen order ID'sine sahip order item'ları getirir.
    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
    {
        return await _context.OrderItems
            .Where(item => item.OrderId == orderId) // Belirtilen orderId'ye sahip order item'ları filtreler
            .ToListAsync(); // Sonuçları bir listeye dönüştürüp asenkron olarak geri döner
    }
}

