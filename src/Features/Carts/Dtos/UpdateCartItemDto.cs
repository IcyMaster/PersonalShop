﻿using PersonalShop.Resources.Validations.CartItem;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Features.Carts.Dtos;

public class UpdateCartItemDto
{
    [Required(ErrorMessageResourceType = typeof(CartItemMessages)
    , ErrorMessageResourceName = nameof(CartItemMessages.QuantityRequired))]
    [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(CartItemDto)
    , ErrorMessageResourceName = nameof(CartItemMessages.QuantityValueRangeError))]
    public int Quantity { get; set; } = 1;
}
