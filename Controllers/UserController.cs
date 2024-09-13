using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IProductService _productService;
        public UserController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        [Route("User/Products")]
        public async Task<ActionResult<IEnumerable<ListOfProductsDto>>> UserProducts()
        {
            return View(await _productService.GetProducts(User.Identity!.GetUserId()));
        }
    }
}
