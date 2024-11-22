using PersonalShop.Features.Categories.Dtos;

namespace PersonalShop.Interfaces.Repositories;

public interface ICategoryQueryRepository
{
    Task<List<SingleCategoryDto>> GetAllCategoriesWithUserAsync();
}
