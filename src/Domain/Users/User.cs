using Microsoft.AspNetCore.Identity;

namespace PersonalShop.Domain.Users;

public class User : IdentityUser
{
    private User()
    {
        //For ef core
    }

    protected User(string email, string userName) : base(userName)
    {

    }

    protected User(string email, string userName, string firstName, string lastName)
        : this(email, userName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static User CreateNew(string email, string username)
    {
        return new User(email, username);
    }

    public static User CreateNewWithFirstNameAndLastName(string email, string username,
        string firstName, string lastName)
    {
        return new User(email, username, firstName, lastName);
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    public void SetFullName(string firstName,string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void SetPhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
}
