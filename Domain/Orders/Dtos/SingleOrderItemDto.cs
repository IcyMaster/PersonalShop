using PersonalShop.Domain.Products;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Orders.Dtos;

public class SingleOrderItemDto
{
    public int OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quanity { get; set; }
    public OrderProductDto Product { get; set; } = null!;
}
