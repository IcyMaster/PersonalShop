using PersonalShop.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Orders.Dtos;

public class SingleOrderDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public OrderUserDto User { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<SingleOrderItemDto> OrderItems { get; set; } = null!;
}
