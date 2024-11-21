using AspNetCore;
using PersonalShop.Domain.Users;

namespace PersonalShop.Domain.Products
{
    public class Product
    {
        public Product()
        {
            //for ef ...
        }
        public Product(string userId,string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public int Id { get; set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; } = decimal.Zero;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeDescription(string description)
        {
            Description = description;
        }

        public void ChangePrice(decimal price)
        {
            Price = price;
        }
    }
}
