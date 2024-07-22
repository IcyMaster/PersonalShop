using Personal_Shop.Domain.Products.DTO;

namespace Personal_Shop.Interfaces;

public interface IProductService
{
    Task<ProductDTO> AddProduct(ProductDTO productModel);
    Task<bool> DeleteProductById(long id);
    Task<ProductDTO?> GetProductById(long id);
    Task<List<ProductDTO>> GetProducts();
    Task<bool> UpdateProductById(long id, ProductDTO productModel);
}

