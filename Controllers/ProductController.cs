using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
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
    public async Task<ActionResult<IEnumerable<SingleProductDto>>> Index()
    {
        return View(await _productService.GetAllProductsWithUserAsync());
    }

    [Authorize]
    [HttpGet]
    [Route("AddProduct")]
    public ActionResult AddProduct()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    [Route("AddProduct")]
    public async Task<ActionResult> AddProduct(CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createProductDto);
        }

        if(await _productService.AddProductByUserIdAsync(createProductDto, User.Identity!.GetUserId()))
        {
            return RedirectToAction(nameof(UserController.UserProducts),"User");
        }

        return BadRequest("Problem to add product in website ...");
    }

    [Authorize]
    [HttpPost]
    [Route("DeleteProduct/{productId:int}", Name = "DeleteProduct")]
    public async Task<ActionResult> DeleteProduct(int productId)
    {
        if (!await _productService.DeleteProductByIdAndValidateOwnerAsync(productId,User.Identity!.GetUserId()))
        {
            return BadRequest("Problem in Delete product");
        }

        return RedirectToAction(nameof(UserController.UserProducts), "User");
    }

    [Authorize]
    [HttpGet]
    [Route("UpdateProduct/{productId:int}", Name = "UpdateProduct")]
    public async Task<ActionResult> UpdateProduct(int productId)
    {
        var product = await _productService.GetProductByIdWithUserAndValidateOwnerAsync(productId,User.Identity!.GetUserId());
        if (product is null)
        {
            return BadRequest("Problem in Load product for edit");
        }

        return View(product);
    }

    [Authorize]
    [HttpPost]
    [Route("UpdateProduct/{productId:int}", Name = "UpdateProduct")]
    public async Task<ActionResult> UpdateProduct(int productId,UpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateProductDto);
        }

        if (!await _productService.UpdateProductByIdAndValidateOwnerAsync(productId, updateProductDto, User.Identity!.GetUserId()))
        {
            return BadRequest("Problem in edit product");
        }

        return RedirectToAction(nameof(UserController.UserProducts), "User");
    }
}
