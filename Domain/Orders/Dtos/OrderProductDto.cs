using PersonalShop.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Orders.Dtos;

public class OrderProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
    public decimal Price { get; set; }
}
