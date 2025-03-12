using EasyCaching.Core;
using MassTransit;
using Microsoft.Extensions.Configuration;
using PersonalShop.BusinessLayer.Builders.Caches;
using PersonalShop.BusinessLayer.Common.Interfaces;
using PersonalShop.BusinessLayer.Services.Carts.Commands;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Products.Commands;
using PersonalShop.BusinessLayer.Services.Products.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Products;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Shared.Contracts;
using PersonalShop.Shared.Resources.Services.ProductService;

namespace PersonalShop.BusinessLayer.Services.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly IConfiguration _config;
    private readonly IBus _bus;

    public ProductService(IProductRepository productRepository,
        IProductQueryRepository productQueryRepository,
        IUnitOfWork unitOfWork, IEasyCachingProvider cachingProvider,
        IBus bus, ICategoryRepository categoryRepository, ITagRepository tagRepository, IConfiguration config)
    {
        _productRepository = productRepository;
        _productQueryRepository = productQueryRepository;
        _unitOfWork = unitOfWork;
        _cachingProvider = cachingProvider;
        _bus = bus;
        _categoryRepository = categoryRepository;
        _tagRepository = tagRepository;
        _config = config;
    }

    public async Task<ServiceResult<string>> CreateProductAsync(CreateProductDto createProductDto, string userId)
    {
        string? fileExtension = Path.GetExtension(createProductDto.Image.FileName);

        if (fileExtension is null || !FileExtensionContracts.SafeExtensions.Contains(fileExtension))
        {
            return ServiceResult<string>.Failed(ProductServiceErrors.ProductImageExtensionProblem);
        }

        // Upload the file if less than 2 MB
        if (createProductDto.Image.Length > 2097152)
        {
            return ServiceResult<string>.Failed(ProductServiceErrors.ProductImageSizeProblem);
        }

        if (!Directory.Exists("wwwroot/" + _config[AppSettingContracts.StoredFilesPath]!))
        {
            Directory.CreateDirectory("wwwroot/" + _config[AppSettingContracts.StoredFilesPath]!);
        }

        var fileName = Path.ChangeExtension(Path.GetRandomFileName(), fileExtension);

        var filePath = Path.Combine("wwwroot/" + _config[AppSettingContracts.StoredFilesPath]!, fileName);

        using (var stream = File.Create(filePath))
        {
            await createProductDto.Image.CopyToAsync(stream);
        }

        var imagePath = $"/{_config[AppSettingContracts.StoredFilesPath]}/{fileName}";

        var newProduct = new Product(
            userId, createProductDto.Name,
            createProductDto.Description, createProductDto.ShortDescription,
            createProductDto.Price, imagePath,createProductDto.Stock);

        if (createProductDto.Categories is not null)
        {
            foreach (int categoryId in createProductDto.Categories)
            {
                var category = await _categoryRepository.GetCategoryDetailsWithoutUserAsync(categoryId);
                if (category is not null)
                {
                    newProduct.Categories.Add(category);
                }
            }
        }

        if (createProductDto.Tags is not null)
        {
            foreach (int tagId in createProductDto.Tags)
            {
                var tag = await _tagRepository.GetTagDetailsWithoutUserAsync(tagId);
                if (tag is not null)
                {
                    newProduct.Tags.Add(tag);
                }
            }
        }

        await _productRepository.AddAsync(newProduct);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);

            return ServiceResult<string>.Success(ProductServiceSuccess.SuccessfulCreateProduct);
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

            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);

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

        if (updateProductDto.Image is not null)
        {
            string? fileExtension = Path.GetExtension(updateProductDto.Image.FileName);

            if (fileExtension is null || !FileExtensionContracts.SafeExtensions.Contains(fileExtension))
            {
                return ServiceResult<string>.Failed(ProductServiceErrors.ProductImageExtensionProblem);
            }

            // Upload the file if less than 2 MB
            if (updateProductDto.Image.Length > 2097152)
            {
                return ServiceResult<string>.Failed(ProductServiceErrors.ProductImageSizeProblem);
            }

            if (!Directory.Exists("wwwroot/" + _config[AppSettingContracts.StoredFilesPath]!))
            {
                Directory.CreateDirectory("wwwroot/" + _config[AppSettingContracts.StoredFilesPath]!);
            }

            var fileName = Path.ChangeExtension(Path.GetRandomFileName(), fileExtension);

            var filePath = Path.Combine("wwwroot/" + _config[AppSettingContracts.StoredFilesPath]!, fileName);

            using (var stream = File.Create(filePath))
            {
                await updateProductDto.Image.CopyToAsync(stream);
            }

            var imagePath = $"/{_config[AppSettingContracts.StoredFilesPath]}/{fileName}";

            var deletePath = $"wwwroot{product.ImagePath}";

            if (File.Exists(deletePath))
            {
                File.Delete(deletePath);
            }

            product.ChangeImage(imagePath);
        }

        product.ChangeName(updateProductDto.Name);
        product.ChangeDescription(updateProductDto.Description);
        product.ChangeShortDescription(updateProductDto.ShortDescription);
        product.ChangePrice(updateProductDto.Price);
        product.ChangeStock(updateProductDto.Stock);

        if (updateProductDto.Categories is not null)
        {
            product.Categories.Clear();
            foreach (int categoryId in updateProductDto.Categories)
            {
                if (!product.Categories.Where(x => x.Id == categoryId).Any())
                {
                    var category = await _categoryRepository.GetCategoryDetailsWithoutUserAsync(categoryId);
                    if (category is not null)
                    {
                        product.Categories.Add(category);
                    }
                }
            }
        }
        else
        {
            product.Categories.Clear();
        }

        if (updateProductDto.Tags is not null)
        {
            product.Tags.Clear();
            foreach (int tagId in updateProductDto.Tags)
            {
                if (!product.Tags.Where(x => x.Id == tagId).Any())
                {
                    var tag = await _tagRepository.GetTagDetailsWithoutUserAsync(tagId);
                    if (tag is not null)
                    {
                        product.Tags.Add(tag);
                    }
                }
            }
        }
        else
        {
            product.Tags.Clear();
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _bus.Publish(new UpdateProductInCartsCommand
            {
                ProductId = productId,
                Price = updateProductDto.Price
            });

            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);

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
    public async Task<ServiceResult<PagedResult<SingleProductDto>>> GetAllProductsWithUserAndValidateOwnerAsync(PagedResultOffset resultOffset, string userId)
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

    //Consume Events Section
    public async Task<ServiceResult<bool>> UpdateProductStockAsync(UpdateProductStockCommand command)
    {
        var product = await _productRepository.GetProductDetailsWithoutUserAsync(command.ProductId);
        if(product is null)
        {
            return ServiceResult<bool>.Failed(ProductServiceErrors.ProductNotFound);
        }

        product.ChangeStock(command.Stock);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);
            return ServiceResult<bool>.Success(true);
        }

        return ServiceResult<bool>.Failed(ProductServiceErrors.UpdateProductStockCommandProblem);
    }
}
