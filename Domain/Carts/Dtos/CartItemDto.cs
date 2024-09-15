using PersonalShop.Domain.Products;

namespace PersonalShop.Domain.Carts.Dtos;

public class CartItemDto
{
    public long ProductId { get; set; }
    public int Quanity { get; set; }
    public CartItemProductDto Product { get; set; } = null!;
}
