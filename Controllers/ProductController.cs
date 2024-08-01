using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Products;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Domain.Products.Dtoss;
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
        var products = await _productService.GetProducts();

        var productsList = products.Select(ob => new ListOfProductsDto
        {
            Id = ob.Id,
            Name = ob.Name,
            Description = ob.Description,
            Price = ob.Price,

            User = new ListOfProductsUserDto
            {
                UserId = ob.UserId,
                FirstName = ob.User.FirstName,
                LastName = ob.User.LastName,
                IsOwner = false,
            }
        }).ToList();

        if (!User.Identity!.IsAuthenticated)
        {
            return View(productsList);
        }
        else
        {
            var user = await _userService.GetUserAsync(User);

            foreach (var item in productsList)
            {
                if(item.User.UserId.Equals(user.Id))
                {
                    item.User.IsOwner = true;
                }
            }

            return View(productsList);
        }
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

        var user = await _userService.GetUserAsync(User);

        Product product = new Product()
        {
            UserId = user.Id,
            Name = createProductDTO.Name,
            Description = createProductDTO.Description,
            Price = createProductDTO.Price,
        };

        await _productService.AddProduct(product);
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [Route("DeleteProduct/{productId:long}", Name = "DeleteProduct")]
    public async Task<ActionResult> DeleteProduct(long productId)
    {
        var product = await _productService.GetProductById(productId);
        if (product is null)
        {
            return View(nameof(Index));
        }

        var user = await _userService.GetUserAsync(User);

        if (!product.UserId.Equals(user.Id))
        {
            return BadRequest("فقط سازنده محصول ، می تواند این محصول را حذف کند");
        }

        await _productService.DeleteProductById(productId);
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpGet]
    [Route("UpdateProduct/{productId:long}", Name = "UpdateProduct")]
    public async Task<ActionResult> UpdateProduct(long productId)
    {
        var product = await _productService.GetProductById(productId);
        if (product is null)
        {
            return BadRequest("محصول مورد نظر یافت نشد");
        }

        var user = await _userService.GetUserAsync(User);

        if (!product.UserId.Equals(user.Id))
        {
            return BadRequest("فقط سازنده محصول ، می تواند این محصول را ویرایش کند");
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

        var product = await _productService.GetProductById(productId);
        if(product is null)
        {
            return BadRequest("محصول مورد نظر یافت نشد");
        }

        var user = await _userService.GetUserAsync(User);

        if (!product.UserId.Equals(user.Id))
        {
            return BadRequest("فقط سازنده محصول ، می تواند این محصول را ویرایش کند");
        }

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;

        await _productService.UpdateProductById(productId,product);
        return RedirectToAction(nameof(Index));
    }
}
