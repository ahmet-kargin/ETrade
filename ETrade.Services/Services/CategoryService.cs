using ETrade.Infrastructure.Connection;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETrade.Services.Services;

// CategoryService, kategori verilerini yönetmek için kullanılan bir servis sınıfıdır.
public class CategoryService
{
    // ApplicationDbContext, veritabanı ile etkileşim kurmak için kullanılan bir DbContext sınıfıdır.
    private readonly ApplicationDbContext _context;

    // Constructor, dependency injection kullanarak ApplicationDbContext'i alır.
    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Kategorileri asenkron olarak döndüren bir metod.
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        // Veritabanındaki tüm kategorileri alır ve bir liste olarak döner.
        return await _context.Categories.ToListAsync();
    }
}


