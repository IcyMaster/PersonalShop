﻿namespace PersonalShop.Features.Carts.Dtos
{
    public class CartItemProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
    }
}