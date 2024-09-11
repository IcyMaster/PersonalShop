namespace PersonalShop.Domain.Card.Dtos;

public class SingleCartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CartItem> CartItems { get; set; } = null!;
}
