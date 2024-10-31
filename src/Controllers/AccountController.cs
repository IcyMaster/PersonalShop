using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Response;
using PersonalShop.Domain.Users.Dtos;
using PersonalShop.Interfaces.Features;

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

        var serviceResult = await _userService.CreateUserAsync(registerDto);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction("Index", "Home");
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
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

        var serviceResult = await _authenticationService.LoginAsync(loginDto.Email, loginDto.Password);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction("Index", "Home");
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize]
    [Route("Logout")]
    public async Task<ActionResult> LogOut()
    {
        var serviceResult = await _authenticationService.LogoutAsync();

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction("Index", "Home");
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }
}

