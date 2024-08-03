using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces;

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
    public async Task<ActionResult<IEnumerable<ListOfProductsDto>>> Index()
    {
        return View(await _productService.GetProducts());
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
    public async Task<ActionResult> AddProduct(CreateProductDTO createProductDTO)
    {
        if (!ModelState.IsValid)
        {
            return View(createProductDTO);
        }

        if(await _productService.AddProduct(createProductDTO,User.Identity!.GetUserId()))
        {
            return RedirectToAction(nameof(UserController.UserProducts),"User");
        }

        return BadRequest("Problem to add product ...");
    }

    [Authorize]
    [HttpPost]
    [Route("DeleteProduct/{productId:long}", Name = "DeleteProduct")]
    public async Task<ActionResult> DeleteProduct(long productId)
    {
        if (!await _productService.DeleteProductById(productId,User.Identity!.GetUserId()))
        {
            return BadRequest("مشکل در حذف محصول مورد نظر");
        }

        return RedirectToAction(nameof(UserController.UserProducts), "User");
    }

    [Authorize]
    [HttpGet]
    [Route("UpdateProduct/{productId:long}", Name = "UpdateProduct")]
    public async Task<ActionResult> UpdateProduct(long productId)
    {
        var product = await _productService.GetProductById(productId,User.Identity!.GetUserId());
        if (product is null)
        {
            return BadRequest("مشکل در بارگذاری محصول");
        }

        return View(product);
    }

    [Authorize]
    [HttpPost]
    [Route("UpdateProduct/{productId:long}", Name = "UpdateProduct")]
    public async Task<ActionResult> UpdateProduct(long productId,UpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateProductDto);
        }

        if (!await _productService.UpdateProductById(productId, updateProductDto, User.Identity!.GetUserId()))
        {
            return BadRequest("مشکل در ویرایش محصول");
        }

        return RedirectToAction(nameof(UserController.UserProducts), "User");
    }
}
