using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Categorys;

namespace PersonalShop.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetCategoryDetailsWithoutUserAsync(int categoryId, bool track = true);
}