using PersonalShop.Domain.Entities.Categorys;
using PersonalShop.Domain.Entities.Tags;
using PersonalShop.Domain.Entities.Users;

namespace PersonalShop.Domain.Entities.Products
{
    public class Product
    {
        private Product()
        {
            //for ef ...
        }
        public Product(string userId, string name, string description, decimal price)
        {
            UserId = userId;
            Name = name;
            Description = description;
            Price = price;
        }

        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; } = decimal.Zero;
        public User User { get; set; } = null!;
        public List<Category> Categories { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];

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
