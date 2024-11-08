using PersonalShop.Domain.Products;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Carts;

public class CartItem
{
    public CartItem(int productId, int quantity, decimal itemPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        ItemPrice = itemPrice;
    }

    [Key]
    public int Id { get; set; }
    public Guid CartId { get; set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal ItemPrice { get; private set; }
    public Product Product { get; } = null!;

    public void IncreaseQuantity(int count)
    {
        Quantity = Quantity + count;
    }
    public void SetQuantity(int count)
    {
        Quantity = count;
    }
    public void SetItemPrice(decimal price)
    {
        ItemPrice = price;
    }
}
