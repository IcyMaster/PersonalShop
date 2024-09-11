using PersonalShop.Domain.Products.Dtos;

namespace PersonalShop.Features.Product;

public interface IProductService
{
    Task<bool> AddProduct(CreateProductDto createProductDto, string userId);
    Task<bool> DeleteProductById(long id, string userId);
    Task<SingleProductDto?> GetProductById(long id);
    Task<SingleProductDto?> GetProductById(long id, string userId);
    Task<List<ListOfProductsDto>> GetProducts();
    Task<List<ListOfProductsDto>> GetProducts(string userId);
    Task<bool> UpdateProductById(long id, UpdateProductDto updateProductDto, string userId);
}

