using Microsoft.EntityFrameworkCore;
using PersonalShop.Data;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Interfaces;

namespace PersonalShop.Features.Product;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddProduct(CreateProductDto createProductDto,string userId)
    {
        await _context.Products.AddAsync(new Domain.Products.Product {
            UserId = userId,
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
        });

        if(await _context.SaveChangesAsync() > 0)
        {
            return true;
        }

        return false;
    }
    public async Task<SingleProductDto?> GetProductById(long id)
    {
        var product = await _context.Products.Include(e => e.User).SingleOrDefaultAsync(e => e.Id.Equals(id));
        if(product is null)
        {
            return null;
        }

        var singleProductDto = new SingleProductDto {
            Id = id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            User = new SingleProductUserDto {
                UserId = product.UserId,
                FirstName = product.User.FirstName,
                LastName = product.User.LastName,
                IsOwner = false
            }
        };

        return singleProductDto;
    }
    public async Task<SingleProductDto?> GetProductById(long id,string userId)
    {
        var product = await _context.Products.Include(e => e.User).SingleOrDefaultAsync(e => e.Id.Equals(id));
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
            User = new SingleProductUserDto
            {
                UserId = product.UserId,
                FirstName = product.User.FirstName,
                LastName = product.User.LastName,
            }
        };

        if(singleProductDto.User.UserId.Equals(userId))
        {
            singleProductDto.User.IsOwner = true;
        }

        return singleProductDto;
    }
    public async Task<List<ListOfProductsDto>> GetProducts()
    {
        return await _context.Products.Include(e => e.User).Select(ob => new ListOfProductsDto
        {
            Id = ob.Id,
            Name = ob.Name,
            Description = ob.Description,
            Price = ob.Price,

            User = new ListOfProductsUserDto
            {
                UserId = ob.User.Id,
                FirstName = ob.User.FirstName,
                LastName = ob.User.LastName,
                IsOwner = false,
            }
        }).ToListAsync();
    }
    public async Task<List<ListOfProductsDto>> GetProducts(string userId)
    {
        var products = await _context.Products.Include(e => e.User).Select(ob => new ListOfProductsDto
        {
            Id = ob.Id,
            Name = ob.Name,
            Description = ob.Description,
            Price = ob.Price,

            User = new ListOfProductsUserDto
            {
                UserId = ob.User.Id,
                FirstName = ob.User.FirstName,
                LastName = ob.User.LastName,
                IsOwner = false,
            }
        }).ToListAsync();

        products.ForEach(ob =>
        {
            if(ob.User.UserId.Equals(userId))
            {
                ob.User.IsOwner = true;
            }
        });

        return products;
    }
    public async Task<bool> DeleteProductById(long id,string userId)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return false;
        }

        if(!product.UserId.Equals(userId))
        {
            return false;
        }

        _context.Products.Remove(product);

        if (await _context.SaveChangesAsync() < 1)
        {
            return false;
        }

        return true;
    }
    public async Task<bool> UpdateProductById(long id,UpdateProductDto updateProductDto,string userId)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return false;
        }

        if(!product.UserId.Equals(userId))
        {
            return false;
        }

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;

        if (await _context.SaveChangesAsync() < 1)
        {
            return false;
        }

        return true;
    }
}
