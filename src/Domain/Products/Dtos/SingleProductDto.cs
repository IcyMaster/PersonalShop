namespace PersonalShop.Domain.Products.Dtos
{
    public class SingleProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public ProductUserDto User { get; set; } = new ProductUserDto();
    }
}
