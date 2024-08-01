using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Users.Dtos;
using PersonalShop.Interfaces;

namespace PersonalShop.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;

    public AccountController(IUserService userService, IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("Register")]
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }

        await _userService.CreateUserAsync(registerDto.UserName,
                                           registerDto.Password,
                                           registerDto.Email,
                                           registerDto.FirstName,
                                           registerDto.LastName,
                                           registerDto.PhoneNumber);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("Login")]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return View(loginDto);
        }
        await _authenticationService.LoginAsync(loginDto.Email, loginDto.Password);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Logout")]
    public async Task<ActionResult> LogOut()
    {
        await _authenticationService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}

