using Microsoft.EntityFrameworkCore;
using PersonalShop.Data;
using PersonalShop.Interfaces;

namespace PersonalShop.Features.Product;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Products.Product> AddProduct(Domain.Products.Product productModel)
    {
        await _context.Products.AddAsync(productModel);
        await _context.SaveChangesAsync();
        return productModel;
    }
    public async Task<Domain.Products.Product?> GetProductById(long id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task<List<Domain.Products.Product>> GetProducts()
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
    public async Task<bool> UpdateProductById(long id,Domain.Products.Product productModel)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return false;
        }

        product = productModel;

        if (await _context.SaveChangesAsync() < 1)
        {
            return false;
        }

        return true;
    }
}
