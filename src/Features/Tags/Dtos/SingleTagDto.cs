namespace PersonalShop.Features.Tags.Dtos;

public class SingleTagDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TagUserDto User { get; set; } = null!;
}
