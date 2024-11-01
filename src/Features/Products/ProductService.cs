using MassTransit;
using PersonalShop.Data.Contracts;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBus _bus;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IBus bus)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _bus = bus;
    }

    public async Task<ServiceResult<string>> CreateProductByUserIdAsync(CreateProductDto createProductDto, string userId)
    {
        await _productRepository.AddAsync(new Domain.Products.Product
        {
            UserId = userId,
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
        });

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return ServiceResult<string>.Success(ProductServiceSuccess.SuccessfulCreateProduct);
        }

        return ServiceResult<string>.Failed(ProductServiceErrors.CreateProductProblem);
    }
    public async Task<ServiceResult<SingleProductDto>> GetProductByIdWithUserAsync(int id)
    {
        var product = await _productRepository.GetProductByIdWithUserAsync(id, track: false);

        if (product is null)
        {
            return ServiceResult<SingleProductDto>.Failed(ProductServiceErrors.ProductNotFound);
        }

        var singleProductDto = new SingleProductDto
        {
            Id = id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            User = new ProductUserDto
            {
                UserId = product.UserId,
                FirstName = product.User.FirstName,
                LastName = product.User.LastName,
                IsOwner = false
            }
        };

        return ServiceResult<SingleProductDto>.Success(singleProductDto);
    }
    public async Task<ServiceResult<SingleProductDto>> GetProductByIdWithOutUserAsync(int id)
    {
        var product = await _productRepository.GetProductByIdWithOutUserAsync(id, track: false);
        if (product is null)
        {
            return ServiceResult<SingleProductDto>.Failed(ProductServiceErrors.ProductNotFound);
        }

        var singleProductDto = new SingleProductDto
        {
            Id = id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            User = new ProductUserDto
            {
                UserId = product.UserId,
                FirstName = product.User.FirstName,
                LastName = product.User.LastName,
                IsOwner = false
            }
        };

        return ServiceResult<SingleProductDto>.Success(singleProductDto);
    }
    public async Task<ServiceResult<SingleProductDto>> GetProductByIdWithUserAndValidateOwnerAsync(int id, string userId)
    {
        var product = await _productRepository.GetProductByIdWithUserAsync(id, track: false);

        if (product is null)
        {
            return ServiceResult<SingleProductDto>.Failed(ProductServiceErrors.ProductNotFound);
        }

        var singleProductDto = new SingleProductDto
        {
            Id = id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            User = new ProductUserDto
            {
                UserId = product.UserId,
                FirstName = product.User.FirstName,
                LastName = product.User.LastName,
            }
        };

        if (singleProductDto.User.UserId.Equals(userId))
        {
            singleProductDto.User.IsOwner = true;
        }

        return ServiceResult<SingleProductDto>.Success(singleProductDto);
    }
    public async Task<ServiceResult<List<SingleProductDto>>> GetAllProductsWithUserAsync()
    {
        var data = await _productRepository.GetAllProductsWithUserAsync(track: false);

        var listOfProduct = data.Select(ob => new SingleProductDto
        {
            Id = ob.Id,
            Name = ob.Name,
            Description = ob.Description,
            Price = ob.Price,

            User = new ProductUserDto
            {
                UserId = ob.User.Id,
                FirstName = ob.User.FirstName,
                LastName = ob.User.LastName,
                IsOwner = false,
            }
        }).ToList();

        return ServiceResult<List<SingleProductDto>>.Success(listOfProduct);
    }
    public async Task<ServiceResult<List<SingleProductDto>>> GetAllProductsWithUserAndValidateOwnerAsync(string userId)
    {
        var data = await _productRepository.GetAllProductsWithUserAsync(track: false);

        var listOfProduct = data.Select(ob => new SingleProductDto
        {
            Id = ob.Id,
            Name = ob.Name,
            Description = ob.Description,
            Price = ob.Price,

            User = new ProductUserDto
            {
                UserId = ob.User.Id,
                FirstName = ob.User.FirstName,
                LastName = ob.User.LastName,
                IsOwner = false,
            }

        }).ToList();

        listOfProduct.ForEach(ob =>
        {
            if (ob.User.UserId.Equals(userId))
            {
                ob.User.IsOwner = true;
            }
        });

        return ServiceResult<List<SingleProductDto>>.Success(listOfProduct);
    }
    public async Task<ServiceResult<string>> DeleteProductByIdAndValidateOwnerAsync(int id, string userId)
    {
        var product = await _productRepository.GetProductByIdWithOutUserAsync(id);

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
                ProductId = id
            });

            return ServiceResult<string>.Success(ProductServiceSuccess.SuccessfulDeleteProduct);
        }

        return ServiceResult<string>.Failed(ProductServiceErrors.DeleteProductProblem);
    }
    public async Task<ServiceResult<string>> UpdateProductByIdAndValidateOwnerAsync(int id, UpdateProductDto updateProductDto, string userId)
    {
        var product = await _productRepository.GetProductByIdWithOutUserAsync(id);
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
                ProductId = id,
                Price = updateProductDto.Price
            });

            return ServiceResult<string>.Success(ProductServiceSuccess.SuccessfulUpdateProduct);
        }

        return ServiceResult<string>.Failed(ProductServiceErrors.UpdateProductProblem);
    }
}
