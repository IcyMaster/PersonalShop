using Personal_Shop.Models.Data;

namespace Personal_Shop.Interfaces;

public interface IProductService
{
    Task<Product> AddProduct(Product productModel);
    Task<bool> DeleteProductById(long id);
    Task<Product?> GetProductById(long id);
    Task<List<Product>> GetProducts();
    Task<bool> UpdateProductById(long id, Product productModel);
}

