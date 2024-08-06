using Microsoft.EntityFrameworkCore;

namespace ETrade.Infrastructure.Connection;

using ETrade.Domain.Entities;
using Framework.ETrade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Her bir entity için tablo adı tanımlamaları
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Category>().ToTable("Categories");
        modelBuilder.Entity<Cart>().ToTable("Carts");
        modelBuilder.Entity<CartItem>().ToTable("CartItems");
        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.PasswordHash).HasMaxLength(200);
        });

        // Varsayılan tablo adlarının ayarlanması
        // Bu metod, tabloların adlarının varsayılanlardan farklı olmasını sağlar
        base.OnModelCreating(modelBuilder);

    }
}

