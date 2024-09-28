using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Features.Product;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AddProductByUserIdAsync(CreateProductDto createProductDto, string userId)
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
            return true;
        }

        return false;
    }
    public async Task<SingleProductDto?> GetProductByIdWithUserAsync(int id)
    {
        var product = await _productRepository.GetProductByIdWithUserAsync(id,track: false);
        if (product is null)
        {
            return null;
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

        return singleProductDto;
    }
    public async Task<SingleProductDto?> GetProductByIdWithOutUserAsync(int id)
    {
        var product = await _productRepository.GetProductByIdWithOutUserAsync(id, track: false);
        if (product is null)
        {
            return null;
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

        return singleProductDto;
    }
    public async Task<SingleProductDto?> GetProductByIdWithUserAndValidateOwnerAsync(int id, string userId)
    {
        var product = await _productRepository.GetProductByIdWithUserAsync(id,track: false);
        if (product is null)
        {
            return null;
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

        return singleProductDto;
    }
    public async Task<List<SingleProductDto>> GetAllProductsWithUserAsync()
    {
        var data = await _productRepository.GetAllProductsWithUserAsync(track: false);

        return data.Select(ob => new SingleProductDto
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
    }
    public async Task<List<SingleProductDto>> GetAllProductsWithUserAndValidateOwnerAsync(string userId)
    {
        var data = await _productRepository.GetAllProductsWithUserAsync(track: false);

        var products = data.Select(ob => new SingleProductDto
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

        products.ForEach(ob =>
        {
            if (ob.User.UserId.Equals(userId))
            {
                ob.User.IsOwner = true;
            }
        });

        return products;
    }
    public async Task<bool> DeleteProductByIdAndValidateOwnerAsync(int id, string userId)
    {
        var product = await _productRepository.GetProductByIdWithOutUserAsync(id);

        if (product is null)
        {
            return false;
        }

        if (!product.UserId.Equals(userId))
        {
            return false;
        }

        _productRepository.Delete(product);

        if (await _unitOfWork.SaveChangesAsync(true) < 1)
        {
            return false;
        }

        return true;
    }
    public async Task<bool> UpdateProductByIdAndValidateOwnerAsync(int id, UpdateProductDto updateProductDto, string userId)
    {
        var product = await _productRepository.GetProductByIdWithOutUserAsync(id);
        if (product is null)
        {
            return false;
        }

        if (!product.UserId.Equals(userId))
        {
            return false;
        }

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;

        if (await _unitOfWork.SaveChangesAsync(true) < 1)
        {
            return false;
        }

        return true;
    }
}
