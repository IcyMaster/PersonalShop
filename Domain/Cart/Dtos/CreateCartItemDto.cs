using PersonalShop.Domain.Products;

namespace PersonalShop.Domain.Card.Dtos;

public class CreateCartItemDto
{
    public Product Product { get; set; } = null!;
    public long ProductId { get; set; }
    public int Quanity { get; set; }
}
