using EasyCaching.Core;
using PersonalShop.BusinessLayer.Builders.Caches;
using PersonalShop.BusinessLayer.Common.Interfaces;
using PersonalShop.BusinessLayer.Services.Categories.Dtos;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Products.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Categorys;
using PersonalShop.Domain.Entities.Products;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Shared.Contracts;
using PersonalShop.Shared.Resources.Services.CategoryService;
using PersonalShop.Shared.Resources.Services.ProductService;

namespace PersonalShop.BusinessLayer.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryQueryRepository _categoryQueryRepository;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepository categoryRepository, ICategoryQueryRepository categoryQueryRepository, IEasyCachingProvider cachingProvider, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _categoryQueryRepository = categoryQueryRepository;
        _cachingProvider = cachingProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<string>> CreateCategoryAsync(string userId, CreateCategoryDto createCategoryDto)
    {
        var newCategory = new Category(userId, createCategoryDto.Name, createCategoryDto.Description, createCategoryDto.ParentId);

        await _categoryRepository.AddAsync(newCategory);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Category);
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
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Category);
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);
            return ServiceResult<string>.Success(CategoryServiceSuccess.SuccessfulUpdateCategory);
        }

        return ServiceResult<string>.Failed(CategoryServiceErrors.UpdateCategoryProblem);
    }
    public async Task<ServiceResult<string>> DeleteCategoryAndValidateOwnerAsync(string userId, int categoryId)
    {
        try
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

            var subCategories = await _categoryQueryRepository.GetAllSubCategoryDetailsWithoutUserAsync(categoryId);

            if (subCategories is not null)
            {
                foreach (var subCategory in subCategories)
                {
                    _categoryRepository.Delete(subCategory);
                }
            }

            _categoryRepository.Delete(category);

            if (await _unitOfWork.SaveChangesAsync(true) > 0)
            {
                await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Category);
                await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);

                return ServiceResult<string>.Success(CategoryServiceSuccess.SuccessfulDeleteCategory);
            }
        }
        catch(Exception ex)
        {

        }
        return ServiceResult<string>.Failed(CategoryServiceErrors.DeleteCategoryProblem);
    }
    public async Task<ServiceResult<SingleCategoryDto>> GetCategoryDetailsWithUserAndValidateOwnerAsync(int categoryId, string userId)
    {
        var category = await _categoryQueryRepository.GetCategoryDetailsWithUserAsync(categoryId);

        if (category is null)
        {
            return ServiceResult<SingleCategoryDto>.Failed(CategoryServiceErrors.CategoryNotFound);
        }

        if (category.User.UserId.Equals(userId))
        {
            category.User.IsOwner = true;
        }

        return ServiceResult<SingleCategoryDto>.Success(category);
    }
    public async Task<ServiceResult<List<SingleCategoryDto>>> GetAllCategoriesWithUserAsync()
    {
        var cache = await _cachingProvider.GetAsync<List<SingleCategoryDto>>(CacheKeysContract.Category);

        if (cache.HasValue)
        {
            return ServiceResult<List<SingleCategoryDto>>.Success(cache.Value);
        }

        var categories = await _categoryQueryRepository.GetAllCategoriesWithUserAsync();

        await _cachingProvider.TrySetAsync(CacheKeysContract.Category, categories, TimeSpan.FromHours(1));

        return ServiceResult<List<SingleCategoryDto>>.Success(categories);
    }
    public async Task<ServiceResult<PagedResult<SingleCategoryDto>>> GetAllCategoriesWithUserAsync(PagedResultOffset resultOffset)
    {
        string cacheKey = CategoryCacheKeyBuilder.CategoryPaginationCacheKey(resultOffset);

        var cache = await _cachingProvider.GetAsync<PagedResult<SingleCategoryDto>>(cacheKey);

        if (cache.HasValue)
        {
            return ServiceResult<PagedResult<SingleCategoryDto>>.Success(cache.Value);
        }

        var categories = await _categoryQueryRepository.GetAllCategoriesWithUserAsync(resultOffset);

        await _cachingProvider.TrySetAsync(cacheKey, categories, TimeSpan.FromHours(1));

        return ServiceResult<PagedResult<SingleCategoryDto>>.Success(categories);
    }
    public async Task<ServiceResult<PagedResult<SingleCategoryDto>>> GetAllCategoriesWithUserAndValidateOwnerAsync(PagedResultOffset resultOffset, string userId)
    {
        var cacheKey = CategoryCacheKeyBuilder.CategoryPaginationCacheKeyWithUserId(userId, resultOffset);

        var cache = await _cachingProvider.GetAsync<PagedResult<SingleCategoryDto>>(cacheKey);

        if (cache.HasValue)
        {
            return ServiceResult<PagedResult<SingleCategoryDto>>.Success(cache.Value);
        }

        var categories = await _categoryQueryRepository.GetAllCategoriesWithUserAsync(resultOffset);

        foreach (var category in categories.Data)
        {
            if (category.User.UserId.Equals(userId))
            {
                category.User.IsOwner = true;
            }
        }

        await _cachingProvider.TrySetAsync(cacheKey, categories, TimeSpan.FromHours(1));

        return ServiceResult<PagedResult<SingleCategoryDto>>.Success(categories);
    }
}
