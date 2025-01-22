using PersonalShop.Domain.Entities.Categorys;

namespace PersonalShop.BusinessLayer.Common.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetCategoryDetailsWithoutUserAsync(int categoryId, bool track = true);
}