using PersonalShop.Domain.Products;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Orders;

public class OrderItem
{
    public OrderItem(int productId,string productName,decimal productPrice,int quanity)
    {
        ProductId = productId;
        ProductName = productName;
        ProductPrice = productPrice;
        Quanity = quanity;
    }

    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public int Quanity { get; set; }
}
