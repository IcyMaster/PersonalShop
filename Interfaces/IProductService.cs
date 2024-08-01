using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Domain.Products;

namespace PersonalShop.Interfaces;

public interface IProductService
{
    Task<Product> AddProduct(Product productModel);
    Task<bool> DeleteProductById(long id);
    Task<Product?> GetProductById(long id);
    Task<List<Product>> GetProducts();
    Task<bool> UpdateProductById(long id,Product productModel);
}

