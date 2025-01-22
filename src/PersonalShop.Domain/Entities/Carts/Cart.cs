namespace PersonalShop.Domain.Entities.Carts;

public class Cart
{
    public Cart(string userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }

    public Guid Id { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public decimal TotalPrice { get; private set; } = decimal.Zero;
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    public void ProcessTotalPrice()
    {
        TotalPrice = CartItems.Select(x => x.ItemPrice * x.Quantity).Sum();
    }
}
