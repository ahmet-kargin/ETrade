using ETrade.Infrastructure.Connection;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETrade.Services.Services;

// ProductService sınıfı, ürünlerle ilgili işlemleri gerçekleştiren bir servis sınıfıdır.
public class ProductService
{
    // ApplicationDbContext, veritabanı işlemlerini gerçekleştirmek için kullanılan bir Entity Framework DbContext sınıfıdır.
    private readonly ApplicationDbContext _context;

    // Constructor, dependency injection kullanarak ApplicationDbContext nesnesini alır.
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Belirli bir kategoriye ait ürünleri asenkron olarak getiren bir metod.
    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        // Kategori ID'sine göre filtrelenmiş ürün listesini veritabanından alır.
        return await _context.Products
                             .Where(p => p.CategoryId == categoryId)  // Belirli bir kategoriye ait ürünleri filtreler
                             .ToListAsync();  // Filtrelenmiş ürünleri liste olarak döner
    }

    // Tüm ürünleri asenkron olarak getiren bir metod.
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        // Tüm ürünleri veritabanından alır ve liste olarak döner.
        return await _context.Products.ToListAsync();
    }
}


