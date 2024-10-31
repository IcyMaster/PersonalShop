using PersonalShop.Domain.Carts.Dtos;

namespace PersonalShop.Domain.Card.Dtos;

public class SingleCartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public int TotalItemCount { get; private set; }
    public List<CartItemDto> CartItems { get; set; } = null!;

    public void ProcessTotalItemCount()
    {
        TotalItemCount = CartItems.Select(x => x.Quanity).Sum();
    }
}
