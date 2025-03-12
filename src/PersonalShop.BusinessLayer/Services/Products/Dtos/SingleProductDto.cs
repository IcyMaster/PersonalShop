namespace PersonalShop.BusinessLayer.Services.Products.Dtos;

public class SingleProductDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal Price { get; set; } = decimal.Zero;
    public string ImagePath { get; set; } = string.Empty;
    public ProductUserDto User { get; set; } = null!;
    public List<ProductCategoryDto> Categories { get; set; } = [];
    public List<ProductTagDto> Tags { get; set; } = [];
    public int Stock { get; set; } = 0;
}
