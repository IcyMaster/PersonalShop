using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Controllers;

[Route("User")]
public class UserController : Controller
{
    private readonly IProductService _productService;
    public UserController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpGet]
    [Route("Products", Name = "UserProducts")]
    public async Task<ActionResult<IEnumerable<SingleProductDto>>> UserProducts()
    {
        return View(await _productService.GetAllProductsWithUserAndValidateOwnerAsync(User.Identity!.GetUserId()));
    }
}
