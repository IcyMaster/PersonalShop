﻿namespace PersonalShop.BusinessLayer.Services.Carts.Dtos;

public class SingleCartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; } = decimal.Zero;
    public int TotalItemCount { get; set; } = 0;
    public List<CartItemDto> CartItems { get; set; } = null!;
}
