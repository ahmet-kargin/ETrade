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

// ProductRepository sınıfı, ürünlerle ilgili veritabanı işlemlerini gerçekleştiren bir repository sınıfıdır.
public class ProductRepository : IProductRepository
{
    // ApplicationDbContext, veritabanı işlemlerini gerçekleştirmek için kullanılan bir Entity Framework DbContext sınıfıdır.
    private readonly ApplicationDbContext _context;

    // Constructor, dependency injection kullanarak ApplicationDbContext nesnesini alır.
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Tüm ürünleri veritabanından asenkron olarak getiren metod
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        // Veritabanındaki tüm ürünleri getirir ve bir liste olarak döner
        return await _context.Products.ToListAsync();
    }

    // Verilen ID'ye sahip ürünü veritabanından asenkron olarak getiren metod
    public async Task<Product> GetByIdAsync(int id)
    {
        // Verilen ID'ye sahip ürünü getirir, ürün bulunamazsa null döner
        return await _context.Products.FindAsync(id);
    }

    // Yeni bir ürünü veritabanına asenkron olarak ekleyen metod
    public async Task AddAsync(Product product)
    {
        // Ürünü veritabanı bağlamına ekler
        _context.Products.Add(product);
        // Değişiklikleri veritabanına kaydeder
        await _context.SaveChangesAsync();
    }

    // Var olan ürünü veritabanında asenkron olarak güncelleyen metod
    public async Task UpdateAsync(Product product)
    {
        // Ürünü veritabanı bağlamında günceller
        _context.Products.Update(product);
        // Değişiklikleri veritabanına kaydeder
        await _context.SaveChangesAsync();
    }

    // Verilen ID'ye sahip ürünü veritabanından asenkron olarak silen metod
    public async Task DeleteAsync(int id)
    {
        // Verilen ID'ye sahip ürünü bulur
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            // Ürünü veritabanı bağlamından çıkarır
            _context.Products.Remove(product);
            // Değişiklikleri veritabanına kaydeder
            await _context.SaveChangesAsync();
        }
    }

    // Belirli bir kategori ID'sine sahip ürünleri veritabanından asenkron olarak getiren metod
    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        // Belirli bir kategori ID'sine sahip tüm ürünleri getirir ve bir liste olarak döner
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }
}


