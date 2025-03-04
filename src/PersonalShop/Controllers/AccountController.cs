using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.BusinessLayer.Services.Identitys.Roles.Dtos;
using PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Shared.Contracts;
using PersonalShop.Shared.Resources.Validations.Controller;

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
    [Authorize(Roles = RolesContract.Owner)]
    public async Task<ActionResult> Index([FromQuery] PagedResultOffset resultOffset)
    {
        var serviceResult = await _userService.GetAllUsersAsync(resultOffset);

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Owner)]
    [Route("ChangeRolePartial")]
    public async Task<ActionResult> GetChangeRolePartial(string userEmail)
    {
        var serviceResult = await _userService.GetUserByEmailAsync(userEmail);

        if (serviceResult.IsSuccess)
        {
            return PartialView("ChangeRolePartial", serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Owner)]
    [Route("roles/AssignRole")]
    public async Task<ActionResult> AssignRole(AssignRoleDto assignRoleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResult<string>.Failed(ControllerMessages.ModelInvalid));
        }

        var serviceResult = await _userService.AssignUserRoleByEmailAsync(assignRoleDto.UserEmail, assignRoleDto.RoleName);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(AccountController.Index), nameof(AccountController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Owner)]
    [Route("roles/RemoveRole")]
    public async Task<ActionResult> removeRole(RemoveRoleDto removeRoleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResult<string>.Failed(ControllerMessages.ModelInvalid));
        }

        var serviceResult = await _userService.RemoveUserRoleByEmailAsync(removeRoleDto.UserEmail, removeRoleDto.RoleName);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(AccountController.Index), nameof(AccountController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
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
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", string.Empty));
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
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", string.Empty));
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
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }
}

