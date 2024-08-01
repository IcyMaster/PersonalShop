using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Users.DTO;
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
    [Route("Register")]
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register(RegisterDTO registerModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerModel);
        }

        await _userService.CreateUserAsync(registerModel.UserName,
                                           registerModel.Password,
                                           registerModel.Email,
                                           registerModel.FirstName,
                                           registerModel.LastName,
                                           registerModel.PhoneNumber);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("Login")]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> Login(LoginDTO loginModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginModel);
        }
        await _authenticationService.LoginAsync(loginModel.Email, loginModel.Password);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<ActionResult> LogOut()
    {
        await _authenticationService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}

