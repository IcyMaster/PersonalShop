using PersonalShop.Domain.Users;

namespace PersonalShop.Domain.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}
