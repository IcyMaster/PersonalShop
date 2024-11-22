using PersonalShop.Features.Categorys.Dtos;

namespace PersonalShop.Interfaces.Repositories;

public interface ICategoryQueryRepository
{
    Task<List<SingleCategoryDto>> GetAllCategorysWithUserAsync();
}
