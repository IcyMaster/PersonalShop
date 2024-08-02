namespace PersonalShop.Domain.Products.Dtos
{
    public class ListOfProductsUserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsOwner { get; set; } = false;
    }
}
