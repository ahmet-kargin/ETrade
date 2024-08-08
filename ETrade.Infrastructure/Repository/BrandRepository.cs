using ETrade.Application.Interfaces;
using ETrade.Domain.Entities;
using ETrade.Infrastructure.Connection;
using Microsoft.EntityFrameworkCore;

namespace ETrade.Infrastructure.Repository;

public class BrandRepository : IBrandRepository
{
    private readonly ApplicationDbContext _context;

    public BrandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Brand>> GetAll()
    {
        return _context.Brands.ToListAsync();
    }
}
