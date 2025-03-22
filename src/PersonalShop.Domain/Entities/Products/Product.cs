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
        public Product(string userId, string name, string description, string shortDescription, decimal price, string imagePath, int stock)
        {
            UserId = userId;
            Name = name;
            ShortDescription = shortDescription;
            Description = description;
            Price = price;
            ImagePath = imagePath;
            Stock = stock;
        }

        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string ShortDescription { get; private set; } = string.Empty;
        public decimal Price { get; private set; } = decimal.Zero;
        public string ImagePath { get; private set; } = string.Empty;
        public User User { get; set; } = null!;
        public List<Category> Categories { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
        public int Stock { get; private set; } = 0;

        public void ChangeName(string name)
        {
            Name = name;
        }
        public void ChangeDescription(string description)
        {
            Description = description;
        }
        public void ChangeShortDescription(string shortDescription)
        {
            ShortDescription = shortDescription;
        }
        public void ChangePrice(decimal price)
        {
            Price = price;
        }
        public void ChangeImage(string imagePath)
        {
            ImagePath = imagePath;
        }
        public void ChangeStock(int stock)
        {
            Stock = stock;
        }
    }
}
