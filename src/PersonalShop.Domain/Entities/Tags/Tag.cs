using PersonalShop.Domain.Entities.Products;
using PersonalShop.Domain.Entities.Users;

namespace PersonalShop.Domain.Entities.Tags;

public class Tag
{
    private Tag()
    {
        //for ef
    }

    public Tag(string userId, string name)
    {
        UserId = userId;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty;
    public User User { get; set; } = null!;
    public List<Product> Products { get; set; } = [];

    public void ChangeName(string name)
    {
        Name = name;
    }
}
