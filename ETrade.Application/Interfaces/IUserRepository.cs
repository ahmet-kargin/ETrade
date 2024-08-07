using ETrade.Domain.Entities;

namespace ETrade.Application.Interfaces;

// Kullanıcılarla ilgili veri erişim operasyonlarını tanımlayan bir arayüz
public interface IUserRepository
{
    // Tüm kullanıcıları asenkron olarak getirir
    // Bu metod, kullanıcıların listesini döner
    Task<IEnumerable<User>> GetAllUsersAsync();
    
    // Verilen ID'ye sahip kullanıcıyı asenkron olarak getirir
    // Bu metod, belirtilen ID'ye sahip bir kullanıcıyı döner, bulunamazsa null döner
    Task<User> GetUserByIdAsync(int id);
    
    // Yeni bir kullanıcıyı asenkron olarak veritabanına ekler
    // Bu metod, yeni bir kullanıcı ekler ve değişiklikleri veritabanına kaydeder
    Task AddUserAsync(User user);
    
    // Var olan bir kullanıcıyı asenkron olarak günceller
    // Bu metod, mevcut bir kullanıcıyı günceller ve değişiklikleri veritabanına kaydeder
    Task UpdateUserAsync(User user);
    
    // Verilen ID'ye sahip kullanıcıyı asenkron olarak veritabanından siler
    // Bu metod, belirtilen ID'ye sahip kullanıcıyı siler ve değişiklikleri veritabanına kaydeder
    Task DeleteUserAsync(int id);
}


