﻿using Framework.ETrade.Domain.Entities;
using System.Threading.Tasks;

namespace ETrade.Application.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task SaveChangesAsync();
    Task<Order> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();

    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
}
