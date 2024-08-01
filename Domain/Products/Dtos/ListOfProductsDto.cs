using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Products.Dtoss;

public class ListOfProductsDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
    public decimal Price { get; set; }

    public ListOfProductsUserDto User { get; set; } = null!;
}

public class ListOfProductsUserDto
{
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsOwner { get; set; } = false;
}
