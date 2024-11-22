using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Categorys;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Categorys.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;
using PersonalShop.Resources.Services.CategoryService;

namespace PersonalShop.Features.Categorys;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryQueryRepository _categoryQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepository categoryRepository, ICategoryQueryRepository categoryQueryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _categoryQueryRepository = categoryQueryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<string>> CreateCategoryAsync(string userId, CreateCategoryDto createCategoryDto)
    {
        var newCategory = new Category(userId, createCategoryDto.Name, createCategoryDto.Description, createCategoryDto.ParentId);

        await _categoryRepository.AddAsync(newCategory);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            //await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);
            return ServiceResult<string>.Success(CategoryServiceSuccess.SuccessfulCreateCategory);
        }

        return ServiceResult<string>.Failed(CategoryServiceErrors.CreateCategoryProblem);
    }
    public async Task<ServiceResult<string>> UpdateCategoryAndValidateOwnerAsync(string userId, int categoryId, UpdateCategoryDto updateCategoryDto)
    {
        var category = await _categoryRepository.GetCategoryDetailsWithoutUserAsync(categoryId);
        if (category is null)
        {
            return ServiceResult<string>.Failed(CategoryServiceErrors.CategoryNotFound);
        }

        if (!category.UserId.Equals(userId))
        {
            return ServiceResult<string>.Failed(CategoryServiceErrors.CategoryOwnerMatchProblem);
        }

        category.ChangeName(updateCategoryDto.Name);
        category.ChangeDescription(updateCategoryDto.Description);
        category.ChangeParentId(updateCategoryDto.ParentId);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            //await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);
            return ServiceResult<string>.Success(CategoryServiceSuccess.SuccessfulCreateCategory);
        }

        return ServiceResult<string>.Failed(CategoryServiceErrors.CreateCategoryProblem);
    }
    public async Task<ServiceResult<string>> DeleteCategoryAndValidateOwnerAsync(string userId, int categoryId)
    {
        var category = await _categoryRepository.GetCategoryDetailsWithoutUserAsync(categoryId);
        if (category is null)
        {
            return ServiceResult<string>.Failed(CategoryServiceErrors.CategoryNotFound);
        }

        if (!category.UserId.Equals(userId))
        {
            return ServiceResult<string>.Failed(CategoryServiceErrors.CategoryOwnerMatchProblem);
        }

        _categoryRepository.Delete(category);

        if (await _unitOfWork.SaveChangesAsync(true) > 1)
        {
            return ServiceResult<string>.Success(CategoryServiceSuccess.SuccessfulUpdateCategory);
        }

        return ServiceResult<string>.Failed(CategoryServiceErrors.UpdateCategoryProblem);
    }
    public async Task<ServiceResult<List<SingleCategoryDto>>> GetAllCategorysWithUserAsync()
    {
        var categorys = await _categoryQueryRepository.GetAllCategorysWithUserAsync();

        return ServiceResult<List<SingleCategoryDto>>.Success(categorys);
    }
    public async Task<ServiceResult<List<SingleCategoryDto>>> GetAllCategorysWithUserAndValidateOwnerAsync(string userId)
    {
        var categorys = await _categoryQueryRepository.GetAllCategorysWithUserAsync();

        foreach (var category in categorys)
        {
            if (category.User.UserId.Equals(userId))
            {
                category.User.IsOwner = true;
            }
        }

        return ServiceResult<List<SingleCategoryDto>>.Success(categorys);
    }
}
