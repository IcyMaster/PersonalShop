namespace PersonalShop.Domain.Categorys;

public class Category
{
    public Category()
    {
        //for ef ...
    }

    public Category(string userId, string name, string description)
    {
        UserId = userId;
        Name = name;
        Description = description;
    }

    public Category(string userId,string name, string description, int parentId)
    {
        UserId = userId;
        Name = name;
        Description = description;
        ParentId = parentId;
    }

    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ParentId { get; private set; } = 0;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
}
