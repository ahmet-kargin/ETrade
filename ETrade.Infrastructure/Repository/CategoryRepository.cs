using ETrade.Application.Interfaces;
using ETrade.Infrastructure.Connection;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETrade.Infrastructure.Repository;


// CategoryRepository sınıfı, kategorilerle ilgili veritabanı işlemlerini gerçekleştiren bir repository sınıfıdır.
public class CategoryRepository : ICategoryRepository
{
    // ApplicationDbContext, veritabanı işlemlerini gerçekleştirmek için kullanılan bir Entity Framework DbContext sınıfıdır.
    private readonly ApplicationDbContext _context;

    // Constructor, dependency injection kullanarak ApplicationDbContext nesnesini alır.
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Tüm kategorileri veritabanından asenkron olarak getiren metod
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        // Veritabanındaki tüm kategorileri getirir ve bir liste olarak döner
        return await _context.Categories.ToListAsync();
    }
}

