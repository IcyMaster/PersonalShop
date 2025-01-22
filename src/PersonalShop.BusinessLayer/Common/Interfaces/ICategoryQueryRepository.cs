using PersonalShop.BusinessLayer.Services.Categories.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Common.Interfaces;

public interface ICategoryQueryRepository
{
    Task<List<SingleCategoryDto>> GetAllCategoriesWithUserAsync();
    Task<PagedResult<SingleCategoryDto>> GetAllCategoriesWithUserAsync(PagedResultOffset resultOffset);
}
