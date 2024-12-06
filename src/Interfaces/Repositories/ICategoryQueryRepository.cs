using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Categories.Dtos;

namespace PersonalShop.Interfaces.Repositories;

public interface ICategoryQueryRepository
{
    Task<List<SingleCategoryDto>> GetAllCategoriesWithUserAsync();
    Task<PagedResult<SingleCategoryDto>> GetAllCategoriesWithUserAsync(PagedResultOffset resultOffset);
}
