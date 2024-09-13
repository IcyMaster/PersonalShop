using PersonalShop.Domain.Products;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Card;

public class CartItem
{
    public CartItem(long productId,int quanity)
    {
        ProductId = productId;
        Quanity = quanity;
    }

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public long ProductId { get; private set; }
    public int Quanity { get; private set; }

    public void IncreaseQuantity(int count)
    {
        Quanity = Quanity + count;
    }
}
