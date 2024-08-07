using Framework.ETrade.Domain.Entities;

namespace ETrade.Application.Interfaces;

public interface ICategoryRepository
{
    //Tüm katergorileri al
    Task<List<Category>> GetAllCategoriesAsync();
}
