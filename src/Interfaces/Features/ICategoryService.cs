using PersonalShop.Domain.Responses;
using PersonalShop.Features.Categorys.Dtos;

namespace PersonalShop.Interfaces.Features;

public interface ICategoryService
{
    Task<ServiceResult<string>> CreateCategoryAsync(string userId, CreateCategoryDto createCategoryDto);
    Task<ServiceResult<string>> DeleteCategoryAndValidateOwnerAsync(string userId, int categoryId);
    Task<ServiceResult<List<SingleCategoryDto>>> GetAllCategorysWithUserAndValidateOwnerAsync(string userId);
    Task<ServiceResult<List<SingleCategoryDto>>> GetAllCategorysWithUserAsync();
    Task<ServiceResult<string>> UpdateCategoryAndValidateOwnerAsync(string userId, int categoryId, UpdateCategoryDto updateCategoryDto);
}