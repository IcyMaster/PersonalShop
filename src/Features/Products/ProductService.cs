using MassTransit;
using PersonalShop.Data.Contracts;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBus _bus;

    public ProductService(IProductRepository productRepository, IProductQueryRepository productQueryRepository,
        IUnitOfWork unitOfWork, IBus bus)
    {
        _productRepository = productRepository;
        _productQueryRepository = productQueryRepository;
        _unitOfWork = unitOfWork;
        _bus = bus;
    }

    public async Task<ServiceResult<string>> CreateProductAsync(CreateProductDto createProductDto, string userId)
    {
        var newProduct = new Product
        {
            UserId = userId,
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
        };

        await _productRepository.AddAsync(newProduct);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return ServiceResult<string>.Success(ProductServiceSuccess.SuccessfulCreateProduct);
        }

        return ServiceResult<string>.Failed(ProductServiceErrors.CreateProductProblem);
    }
    public async Task<ServiceResult<string>> DeleteProductAndValidateOwnerAsync(int productId, string userId)
    {
        var product = await _productRepository.GetProductDetailsWithOutUserAsync(productId);

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
        var product = await _productRepository.GetProductDetailsWithOutUserAsync(productId);
        if (product is null)
        {
            return ServiceResult<string>.Failed(ProductServiceErrors.ProductNotFound);
        }

        if (!product.UserId.Equals(userId))
        {
            return ServiceResult<string>.Failed(ProductServiceErrors.ProductOwnerMatchProblem);
        }

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;

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
        var product = await _productQueryRepository.GetProductDetailsWithOutUserAsync(productId);
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
    public async Task<ServiceResult<List<SingleProductDto>>> GetAllProductsWithUserAsync()
    {
        var products = await _productQueryRepository.GetAllProductsWithUserAsync();

        return ServiceResult<List<SingleProductDto>>.Success(products);
    }
    public async Task<ServiceResult<List<SingleProductDto>>> GetAllProductsWithUserAndValidateOwnerAsync(string userId)
    {
        var products = await _productQueryRepository.GetAllProductsWithUserAsync();

        foreach (var product in products)
        {
            if (product.User.UserId.Equals(userId))
            {
                product.User.IsOwner = true;
            }
        }

        return ServiceResult<List<SingleProductDto>>.Success(products);
    }
}
