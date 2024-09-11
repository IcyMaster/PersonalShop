using PersonalShop.Domain.Products;

namespace PersonalShop.Domain.Card;

public class CartItem
{
    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public long ProductId { get; set; }
    public int Quanity { get; set; }
}
