namespace PersonalShop.BusinessLayer.Services.Carts.Dtos;

public class CartItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 0;
    public CartItemProductDto Product { get; set; } = null!;
}
