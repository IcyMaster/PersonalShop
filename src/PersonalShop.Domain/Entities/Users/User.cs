using Microsoft.AspNetCore.Identity;

namespace PersonalShop.Domain.Entities.Users;

public class User : IdentityUser
{
    private User()
    {
        //For ef core
    }

    protected User(string email, string userName) : base(userName)
    {
        Email = email;
    }

    protected User(string email, string userName, string? firstName, string? lastName)
        : this(email, userName)
    {
        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    protected User(string email, string userName, string? firstName, string? lastName, string? phoneNumber)
    : this(email, userName)
    {
        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        {
            FirstName = firstName;
            LastName = lastName;
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            PhoneNumber = phoneNumber;
        }
    }

    public static User CreateNew(string email, string username)
    {
        return new User(email, username);
    }

    public static User CreateNewWithFullName(string email, string username,
    string? firstName, string? lastName)
    {
        return new User(email, username, firstName, lastName);
    }

    public static User CreateNewWithFullNameAndPhoneNumber(string email, string username,
    string? firstName, string? lastName, string? phoneNumber)
    {
        return new User(email, username, firstName, lastName, phoneNumber);
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
}
