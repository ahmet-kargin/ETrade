using Framework.ETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETrade.Application.Interfaces;


// Ürünlerle ilgili veri erişim operasyonlarını tanımlayan bir arayüz
public interface IProductRepository
{
    // Tüm ürünleri asenkron olarak getirir
    // Bu metod, ürünlerin listesini döner
    Task<IEnumerable<Product>> GetAllAsync();
    
    // Verilen ID'ye sahip ürünü asenkron olarak getirir
    // Bu metod, belirtilen ID'ye sahip bir ürünü döner, bulunamazsa null döner
    Task<Product> GetByIdAsync(int id);
    
    // Yeni bir ürünü asenkron olarak veritabanına ekler
    // Bu metod, yeni bir ürün ekler ve değişiklikleri veritabanına kaydeder
    Task AddAsync(Product product);
    
    // Var olan bir ürünü asenkron olarak günceller
    // Bu metod, mevcut bir ürünü günceller ve değişiklikleri veritabanına kaydeder
    Task UpdateAsync(Product product);
    
    // Verilen ID'ye sahip ürünü asenkron olarak veritabanından siler
    // Bu metod, belirtilen ID'ye sahip ürünü siler ve değişiklikleri veritabanına kaydeder
    Task DeleteAsync(int id);
    
    // Verilen kategori ID'sine sahip ürünleri asenkron olarak getirir
    // Bu metod, belirtilen kategori ID'sine sahip ürünlerin listesini döner
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
}



