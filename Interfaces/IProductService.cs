using PersonalShop.Domain.Products.DTO;

namespace PersonalShop.Interfaces;

public interface IProductService
{
    Task<ProductDTO> AddProduct(ProductDTO productModel);
    Task<bool> DeleteProductById(long id);
    Task<ProductDTO?> GetProductById(long id);
    Task<List<ProductDTO>> GetProducts();
    Task<bool> UpdateProductById(long id, ProductDTO productModel);
}

