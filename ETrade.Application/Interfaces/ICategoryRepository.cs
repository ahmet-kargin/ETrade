using Framework.ETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETrade.Application.Interfaces;

public interface ICategoryRepository
{
    //Tüm katergorileri al
    Task<List<Category>> GetAllCategoriesAsync();
}
