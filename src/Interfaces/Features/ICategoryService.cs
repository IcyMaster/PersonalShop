using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Categories.Dtos;

namespace PersonalShop.Interfaces.Features;

public interface ICategoryService
{
    Task<ServiceResult<string>> CreateCategoryAsync(string userId, CreateCategoryDto createCategoryDto);
    Task<ServiceResult<string>> DeleteCategoryAndValidateOwnerAsync(string userId, int categoryId);
    Task<ServiceResult<string>> UpdateCategoryAndValidateOwnerAsync(string userId, int categoryId, UpdateCategoryDto updateCategoryDto);
    Task<ServiceResult<List<SingleCategoryDto>>> GetAllCategoriesWithUserAsync();
    Task<ServiceResult<PagedResult<SingleCategoryDto>>> GetAllCategoriesWithUserAsync(PagedResultOffset resultOffset);
    Task<ServiceResult<PagedResult<SingleCategoryDto>>> GetAllCategoriesWithUserAndValidateOwnerAsync(PagedResultOffset resultOffset,string userId);
}