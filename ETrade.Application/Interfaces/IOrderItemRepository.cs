using Framework.ETrade.Domain.Entities;

namespace ETrade.Application.Interfaces;

public interface IOrderItemRepository
{
    Task AddAsync(OrderItem orderItem);
    Task SaveChangesAsync();
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
}
