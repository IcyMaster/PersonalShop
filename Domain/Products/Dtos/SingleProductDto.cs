using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Products.Dtos
{
    public class SingleProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public SingleProductUserDto User { get; set; } = null!;
    }
}
