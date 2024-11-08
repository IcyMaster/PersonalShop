using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Extension;
using PersonalShop.Features.Products.Dtos;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Controllers;

[Route("Products")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IUserService _userService;

    public ProductController(IProductService productService, IUserService userService)
    {
        _productService = productService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<SingleProductDto>>> Index([FromQuery] PagedResultOffset resultOffset)
    {
        var validateObject = ObjectValidatorExtension.Validate(resultOffset);
        if (!validateObject.IsValid)
        {
            return BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
        }

        var serviceResult = await _productService.GetAllProductsWithUserAsync(resultOffset);

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpGet]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("AddProduct")]
    public ActionResult AddProduct()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("AddProduct")]
    public async Task<ActionResult> AddProduct(CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createProductDto);
        }

        var serviceResult = await _productService.CreateProductAsync(createProductDto, User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserProducts), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("DeleteProduct/{productId:int}", Name = "DeleteProduct")]
    public async Task<ActionResult> DeleteProduct(int productId)
    {
        var serviceResult = await _productService.DeleteProductAndValidateOwnerAsync(productId, User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserProducts), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpGet]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("UpdateProduct/{productId:int}", Name = "UpdateProduct")]
    public async Task<ActionResult> UpdateProduct(int productId)
    {
        var serviceResult = await _productService.GetProductDetailsWithUserAndValidateOwnerAsync(productId, User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("UpdateProduct/{productId:int}", Name = "UpdateProduct")]
    public async Task<ActionResult> UpdateProduct(int productId, UpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateProductDto);
        }

        var serviceResult = await _productService.UpdateProductAndValidateOwnerAsync(productId, updateProductDto, User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserProducts), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }
}
