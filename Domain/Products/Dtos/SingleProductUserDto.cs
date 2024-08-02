namespace PersonalShop.Domain.Products.Dtos
{
    public class SingleProductUserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsOwner { get; set; } = false;
    }
}
