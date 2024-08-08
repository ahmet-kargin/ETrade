using ETrade.Application.Interfaces;
using ETrade.Infrastructure.Connection;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETrade.Infrastructure.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    // Constructor: ApplicationDbContext'i dependency injection ile alır.
    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    // Yeni bir order ekler.
    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }
    // Veritabanına yapılan değişiklikleri kaydeder.
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    // Belirtilen ID'ye sahip order'ı getirir.
    public async Task<Order> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }
    // Tüm order'ları getirir.
    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    // Belirtilen kullanıcı ID'sine sahip tüm order'ları getirir.
    // OrderItems ve her OrderItem içindeki Product bilgilerini de dahil eder.
    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _context.Orders
            .Where(order => order.UserId == userId) // Belirtilen userId'ye sahip order'ları filtreler
            .Include(order => order.OrderItems) // OrderItems'ları dahil eder
            .ThenInclude(oi => oi.Product) // Her OrderItem içindeki Product bilgilerini dahil eder
            .ToListAsync();
    }
}
