using PersonalShop.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Products
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}
