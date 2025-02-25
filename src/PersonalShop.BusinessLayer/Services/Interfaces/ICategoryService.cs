using PersonalShop.BusinessLayer.Services.Categories.Dtos;
using PersonalShop.BusinessLayer.Services.Products.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Services.Interfaces;

public interface ICategoryService
{
    Task<ServiceResult<string>> CreateCategoryAsync(string userId, CreateCategoryDto createCategoryDto);
    Task<ServiceResult<string>> DeleteCategoryAndValidateOwnerAsync(string userId, int categoryId);
    Task<ServiceResult<string>> UpdateCategoryAndValidateOwnerAsync(string userId, int categoryId, UpdateCategoryDto updateCategoryDto);
    Task<ServiceResult<SingleCategoryDto>> GetCategoryDetailsWithUserAndValidateOwnerAsync(int categoryId, string userId);
    Task<ServiceResult<List<SingleCategoryDto>>> GetAllCategoriesWithUserAsync();
    Task<ServiceResult<PagedResult<SingleCategoryDto>>> GetAllCategoriesWithUserAsync(PagedResultOffset resultOffset);
    Task<ServiceResult<PagedResult<SingleCategoryDto>>> GetAllCategoriesWithUserAndValidateOwnerAsync(PagedResultOffset resultOffset, string userId);
}