using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Personal_Shop.Data;
using Personal_Shop.Domain.Products.DTO;
using Personal_Shop.Domain.Users;
using Personal_Shop.Interfaces;

namespace Personal_Shop.Features.Product;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDTO> AddProduct(ProductDTO productModel)
    {
        await _context.Products.AddAsync(productModel);
        await _context.SaveChangesAsync();
        return productModel;
    }
    public async Task<ProductDTO?> GetProductById(long id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task<List<ProductDTO>> GetProducts()
    {
        return await _context.Products.Include(e => e.User).ToListAsync();
    }
    public async Task<bool> DeleteProductById(long id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
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
    public async Task<bool> UpdateProductById(long id, ProductDTO productModel)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return false;
        }

        product.Name = productModel.Name;
        product.Description = productModel.Description;
        product.Price = productModel.Price;

        if (await _context.SaveChangesAsync() < 1)
        {
            return false;
        }

        return true;
    }
}
