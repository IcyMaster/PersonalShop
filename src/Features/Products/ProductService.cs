using EasyCaching.Core;
using MassTransit;
using PersonalShop.Builders.Caches;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Categorys;
using PersonalShop.Domain.Products;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Carts.Commands;
using PersonalShop.Features.Products.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;
using PersonalShop.Resources.Services.ProductService;

namespace PersonalShop.Features.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly IBus _bus;

    public ProductService(IProductRepository productRepository, IProductQueryRepository productQueryRepository,
        IUnitOfWork unitOfWork, IEasyCachingProvider cachingProvider, IBus bus, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _productQueryRepository = productQueryRepository;
        _unitOfWork = unitOfWork;
        _cachingProvider = cachingProvider;
        _bus = bus;
        _categoryRepository = categoryRepository;
    }

    public async Task<ServiceResult<string>> CreateProductAsync(CreateProductDto createProductDto, string userId)
    {
        try
        {
            var newProduct = new Product(userId, createProductDto.Name,
                createProductDto.Description, createProductDto.Price);

            if (createProductDto.Categories is not null)
            {
                List<Category> categories = new List<Category>();

                foreach (int categoryId in createProductDto.Categories)
                {
                    var category = await _categoryRepository.GetCategoryDetailsWithoutUserAsync(categoryId);
                    if (category is not null)
                    {
                        newProduct.AddCategory(category);
                    }
                }
            }

            await _productRepository.AddAsync(newProduct);

            if (await _unitOfWork.SaveChangesAsync(true) > 0)
            {
                await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);
                return ServiceResult<string>.Success(ProductServiceSuccess.SuccessfulCreateProduct);
            }
        }
        catch (Exception ex)
        {

        }
        return ServiceResult<string>.Failed(ProductServiceErrors.CreateProductProblem);
    }
    public async Task<ServiceResult<string>> DeleteProductAndValidateOwnerAsync(int productId, string userId)
    {
        var product = await _productRepository.GetProductDetailsWithoutUserAsync(productId);

        if (product is null)
        {
            return ServiceResult<string>.Failed(ProductServiceErrors.ProductNotFound);
        }

        if (!product.UserId.Equals(userId))
        {
            return ServiceResult<string>.Failed(ProductServiceErrors.ProductOwnerMatchProblem);
        }

        _productRepository.Delete(product);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _bus.Publish(new DeleteProductFromCartCommand
            {
                ProductId = productId
            });

            return ServiceResult<string>.Success(ProductServiceSuccess.SuccessfulDeleteProduct);
        }

        return ServiceResult<string>.Failed(ProductServiceErrors.DeleteProductProblem);
    }
    public async Task<ServiceResult<string>> UpdateProductAndValidateOwnerAsync(int productId, UpdateProductDto updateProductDto, string userId)
    {
        var product = await _productRepository.GetProductDetailsWithoutUserAsync(productId);
        if (product is null)
        {
            return ServiceResult<string>.Failed(ProductServiceErrors.ProductNotFound);
        }

        if (!product.UserId.Equals(userId))
        {
            return ServiceResult<string>.Failed(ProductServiceErrors.ProductOwnerMatchProblem);
        }

        product.ChangeName(updateProductDto.Name);
        product.ChangeDescription(updateProductDto.Description);
        product.ChangePrice(updateProductDto.Price);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _bus.Publish(new UpdateProductInCartsCommand
            {
                ProductId = productId,
                Price = updateProductDto.Price
            });

            return ServiceResult<string>.Success(ProductServiceSuccess.SuccessfulUpdateProduct);
        }

        return ServiceResult<string>.Failed(ProductServiceErrors.UpdateProductProblem);
    }
    public async Task<ServiceResult<SingleProductDto>> GetProductDetailsWithUserAsync(int productId)
    {
        var product = await _productQueryRepository.GetProductDetailsWithUserAsync(productId);

        if (product is null)
        {
            return ServiceResult<SingleProductDto>.Failed(ProductServiceErrors.ProductNotFound);
        }

        return ServiceResult<SingleProductDto>.Success(product);
    }
    public async Task<ServiceResult<SingleProductDto>> GetProductDetailsWithOutUserAsync(int productId)
    {
        var product = await _productQueryRepository.GetProductDetailsWithoutUserAsync(productId);
        if (product is null)
        {
            return ServiceResult<SingleProductDto>.Failed(ProductServiceErrors.ProductNotFound);
        }

        return ServiceResult<SingleProductDto>.Success(product);
    }
    public async Task<ServiceResult<SingleProductDto>> GetProductDetailsWithUserAndValidateOwnerAsync(int productId, string userId)
    {
        var product = await _productQueryRepository.GetProductDetailsWithUserAsync(productId);

        if (product is null)
        {
            return ServiceResult<SingleProductDto>.Failed(ProductServiceErrors.ProductNotFound);
        }

        if (product.User.UserId.Equals(userId))
        {
            product.User.IsOwner = true;
        }

        return ServiceResult<SingleProductDto>.Success(product);
    }
    public async Task<ServiceResult<PagedResult<SingleProductDto>>> GetAllProductsWithUserAsync(PagedResultOffset resultOffset)
    {
        string cacheKey = ProductCacheKeyBuilder.ProductPaginationCacheKey(resultOffset);

        var cache = await _cachingProvider.GetAsync<PagedResult<SingleProductDto>>(cacheKey);

        if (cache.HasValue)
        {
            return ServiceResult<PagedResult<SingleProductDto>>.Success(cache.Value);
        }

        var products = await _productQueryRepository.GetAllProductsWithUserAsync(resultOffset);

        await _cachingProvider.TrySetAsync(cacheKey, products, TimeSpan.FromHours(1));

        return ServiceResult<PagedResult<SingleProductDto>>.Success(products);
    }
    public async Task<ServiceResult<PagedResult<SingleProductDto>>> GetAllProductsWithUserAndValidateOwnerAsync(string userId, PagedResultOffset resultOffset)
    {
        string cacheKey = ProductCacheKeyBuilder.ProductPaginationCacheKeyWithUserId(userId, resultOffset);

        var cache = await _cachingProvider.GetAsync<PagedResult<SingleProductDto>>(cacheKey);

        if (cache.HasValue)
        {
            return ServiceResult<PagedResult<SingleProductDto>>.Success(cache.Value);
        }

        var products = await _productQueryRepository.GetAllProductsWithUserAsync(resultOffset);

        foreach (var product in products.Data)
        {
            if (product.User.UserId.Equals(userId))
            {
                product.User.IsOwner = true;
            }
        }

        await _cachingProvider.TrySetAsync(cacheKey, products, TimeSpan.FromHours(1));

        return ServiceResult<PagedResult<SingleProductDto>>.Success(products);
    }
}
