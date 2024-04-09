using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Personal_Shop.Data;
using Personal_Shop.Interfaces;
using Personal_Shop.Models.Data;

namespace Personal_Shop.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> AddProduct(Product productModel)
    {
        await _context.AddAsync(productModel);
        await _context.SaveChangesAsync();
        return productModel;
    }
    public async Task<Product?> GetProductById(long id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task<List<Product>> GetProducts()
    {
        return await _context.Products.ToListAsync();
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
    public async Task<bool> UpdateProductById(long id, Product productModel)
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
