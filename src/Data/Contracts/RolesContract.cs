namespace PersonalShop.Data.Contracts;

internal static class RolesContract
{
    public const string Owner = "Owner";
    public const string Admin = "Admin";
    public const string Author = "Author";
    public const string Customer = "Customer";

    public static List<string> Roles
    {
        get
        {
            return new List<string> {
                Owner,Admin, Author, Customer
            };
        }
    }
}
