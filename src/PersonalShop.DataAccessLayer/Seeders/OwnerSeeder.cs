using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;
using PersonalShop.DataAccessLayer.Contracts;
using PersonalShop.Domain.Entities.Users;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.DataAccessLayer.Seeders;

public sealed class OwnerSeeder : IDataBaseSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<OwnerSeeder> _logger;
    private readonly IConfiguration _config;

    public OwnerSeeder(UserManager<User> userManager, ILogger<OwnerSeeder> logger, IConfiguration config)
    {
        _userManager = userManager;
        _logger = logger;
        _config = config;
    }

    public async Task<bool> MigrateAsync()
    {

        var ownerUserDto = UserRegisterDto();

        var userExist = await _userManager.Users.Where(x => x.Email == ownerUserDto.Email).AnyAsync();

        if (userExist)
        {
            return true;
        }

        _logger.LogInformation("Owner Seeder Migration Started!");

        var ownerUser = User.CreateNewWithFullNameAndPhoneNumber(ownerUserDto.Email, ownerUserDto.UserName,
            ownerUserDto.FirstName, ownerUserDto.LastName, ownerUserDto.PhoneNumber);

        try
        {
            await _userManager.CreateAsync(ownerUser, ownerUserDto.Password);

            //It can be placed in a UserRole Seeder 
            await _userManager.AddToRoleAsync(ownerUser, RolesContract.Owner);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error While Migration Owner Seeder");
            return false;
        }

        _logger.LogInformation("Owner Seeder Migration Has Finished!");
        return true;
    }

    private RegisterDto UserRegisterDto()
    {
        return new RegisterDto
        {
            Email = _config.GetSection("OwnerAccountInfo:Email").Value!,
            FirstName = _config.GetSection("OwnerAccountInfo:FirstName").Value!,
            LastName = _config.GetSection("OwnerAccountInfo:LastName").Value!,
            UserName = _config.GetSection("OwnerAccountInfo:UserName").Value!,
            Password = _config.GetSection("OwnerAccountInfo:Password").Value!,
            PhoneNumber = _config.GetSection("OwnerAccountInfo:PhoneNumber").Value!,
        };
    }
}
