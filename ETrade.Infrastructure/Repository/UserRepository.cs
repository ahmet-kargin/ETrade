using ETrade.Application.Interfaces;
using ETrade.Domain.Entities;
using ETrade.Infrastructure.Connection;
using Microsoft.EntityFrameworkCore;

namespace ETrade.Infrastructure.Repository;

// UserRepository sınıfı, kullanıcılarla ilgili veritabanı işlemlerini gerçekleştiren bir repository sınıfıdır.
public class UserRepository : IUserRepository
{
    // ApplicationDbContext, veritabanı işlemlerini gerçekleştirmek için kullanılan bir Entity Framework DbContext sınıfıdır.
    private readonly ApplicationDbContext _context;

    // Constructor, dependency injection kullanarak ApplicationDbContext nesnesini alır.
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Tüm kullanıcıları veritabanından asenkron olarak getiren metod
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        // Veritabanındaki tüm kullanıcıları getirir ve bir liste olarak döner
        return await _context.Users.ToListAsync();
    }

    // Verilen ID'ye sahip kullanıcıyı veritabanından asenkron olarak getiren metod
    public async Task<User> GetUserByIdAsync(int id)
    {
        // Verilen ID'ye sahip kullanıcıyı getirir, kullanıcı bulunamazsa null döner
        return await _context.Users.FindAsync(id);
    }

    // Yeni bir kullanıcıyı veritabanına asenkron olarak ekleyen metod
    public async Task AddUserAsync(User user)
    {
        // Kullanıcıyı veritabanı bağlamına ekler
        await _context.Users.AddAsync(user);
        // Değişiklikleri veritabanına kaydeder
        await _context.SaveChangesAsync();
    }

    // Var olan kullanıcıyı veritabanında asenkron olarak güncelleyen metod
    public async Task UpdateUserAsync(User user)
    {
        // Kullanıcıyı veritabanı bağlamında günceller
        _context.Users.Update(user);
        // Değişiklikleri veritabanına kaydeder
        await _context.SaveChangesAsync();
    }

    // Verilen ID'ye sahip kullanıcıyı veritabanından asenkron olarak silen metod
    public async Task DeleteUserAsync(int id)
    {
        // Verilen ID'ye sahip kullanıcıyı bulur
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            // Kullanıcıyı veritabanı bağlamından çıkarır
            _context.Users.Remove(user);
            // Değişiklikleri veritabanına kaydeder
            await _context.SaveChangesAsync();
        }
    }
}

