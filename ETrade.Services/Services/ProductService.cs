using ETrade.Infrastructure.Connection;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETrade.Services.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _context.Products
                             .Where(p => p.CategoryId == categoryId)
                             .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }
}

