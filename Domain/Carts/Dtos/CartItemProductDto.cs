using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Carts.Dtos
{
    public class CartItemProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
    }
}
