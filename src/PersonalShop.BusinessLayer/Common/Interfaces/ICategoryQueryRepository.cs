using PersonalShop.BusinessLayer.Services.Categories.Dtos;
using PersonalShop.BusinessLayer.Services.Products.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Categorys;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Common.Interfaces;

public interface ICategoryQueryRepository
{
    Task<List<SingleCategoryDto>> GetAllCategoriesWithUserAsync();
    Task<SingleCategoryDto?> GetCategoryDetailsWithUserAsync(int categoryId);
    Task<PagedResult<SingleCategoryDto>> GetAllCategoriesWithUserAsync(PagedResultOffset resultOffset);
    Task<List<Category>?> GetAllSubCategoryDetailsWithoutUserAsync(int parentId, bool track = true);
}
