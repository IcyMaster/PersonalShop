using Microsoft.AspNetCore.Mvc;
using Personal_Shop.Interfaces;
using Personal_Shop.Models.Data;
using Personal_Shop.Services;

namespace Personal_Shop.Controllers;

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

    #region Register User
    [HttpGet]
    [Route("Register")]
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register(Register registerModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerModel);
        }

        try
        {
            await _userService.CreateUserAsync(registerModel.UserName,
                                               registerModel.Password,
                                               registerModel.Email,
                                               registerModel.FirstName,
                                               registerModel.LastName,
                                               registerModel.PhoneNumber);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception)
        {
            return View(registerModel);
        }
    }
    #endregion

    #region Login User
    [HttpGet]
    [Route("Login")]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> Login(Login loginModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginModel);
        }

        try
        {
            await _authenticationService.LoginAsync(loginModel.Email,loginModel.Password);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
}

