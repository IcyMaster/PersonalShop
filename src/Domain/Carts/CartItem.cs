using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Products;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Card;

public class CartItem
{
    public CartItem(int productId, int quanity, decimal itemPrice)
    {
        ProductId = productId;
        Quanity = quanity;
        ItemPrice = itemPrice;
    }

    [Key]
    public int Id { get; set; }
    public Guid CartId { get; set; }
    public Product Product { get; set; } = null!;
    public int ProductId { get; private set; }
    public int Quanity { get; private set; }
    public decimal ItemPrice { get; private set; }

    public void IncreaseQuantity(int count)
    {
        Quanity = Quanity + count;
    }
    public void SetQuantity(int count)
    {
        Quanity = count;
    }
    public void SetItemPrice(decimal price)
    {
        ItemPrice = price;
    }
}
