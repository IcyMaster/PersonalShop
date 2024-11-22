namespace PersonalShop.Features.Categorys.Dtos;

public class SingleCategoryDto
{
    public long Id { get; set; }
    public int ParentId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public CategoryUserDto User { get; set; } = null!;
}
