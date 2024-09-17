using PersonalShop.Domain.Products;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Orders;

public class OrderItem
{
    public OrderItem(int productId,int quanity)
    {
        ProductId = productId;
        Quanity = quanity;
    }

    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quanity { get; set; }
    public Product Product { get; set; } = null!;
}
