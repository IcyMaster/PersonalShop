﻿namespace PersonalShop.Features.Carts.Commands;

public class UpdateProductInCartsCommand
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
}