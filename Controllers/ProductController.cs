using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Personal_Shop.Interfaces;
using Personal_Shop.Models.Data;

namespace Personal_Shop.Controllers
{
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Index()
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
        public async Task<ActionResult> AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.CreatorName = User.Identity!.Name!;

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

            if (!User.Identity!.Name!.Equals(product.CreatorName))
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
                return View(nameof(Index));
            }

            if (!User.Identity!.Name!.Equals(product.CreatorName))
            {
                return BadRequest("فقط سازنده محصول ، می تواند این محصول را ویرایش کند");
            }

            return View(product);
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateProduct/{productId:long}", Name = "UpdateProduct")]
        public async Task<ActionResult> UpdateProduct(long productId, Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _productService.UpdateProductById(productId, product);
            return RedirectToAction(nameof(Index));
        }
    }
}
