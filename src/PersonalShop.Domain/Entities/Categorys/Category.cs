using PersonalShop.Domain.Entities.Products;
using PersonalShop.Domain.Entities.Users;

namespace PersonalShop.Domain.Entities.Categorys;

public class Category
{
    private Category()
    {
        //for ef ...
    }
    public Category(string userId, string name, string description)
    {
        UserId = userId;
        Name = name;
        Description = description;
    }
    public Category(string userId, string name, string description, int parentId)
    {
        UserId = userId;
        Name = name;
        Description = description;
        ParentId = parentId;
    }

    public int Id { get; set; }
    public int ParentId { get; private set; } = 0;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty;
    public User User { get; set; } = null!;
    public List<Product> Products { get; set; } = [];

    public void ChangeParentId(int parentId)
    {
        ParentId = parentId;
    }
    public void ChangeName(string name)
    {
        Name = name;
    }
    public void ChangeDescription(string description)
    {
        Description = description;
    }
}
