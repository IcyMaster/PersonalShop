namespace PersonalShop.Features.Carts.Dtos;

public class SingleCartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; } = decimal.Zero;
    public int TotalItemCount { get; private set; } = 0;
    public List<CartItemDto> CartItems { get; set; } = null!;

    public void ProcessTotalItemCount()
    {
        TotalItemCount = CartItems.Select(x => x.Quantity).Sum();
    }
}
