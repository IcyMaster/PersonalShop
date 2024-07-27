using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Personal_Shop.Domain.Products.DTO;
using Personal_Shop.Interfaces;

namespace Personal_Shop.Controllers
{
    [Route("Products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IHttpClientFactory _httpFactory;

        public ProductController(IProductService productService, IUserService userService, IHttpClientFactory httpFactory)
        {
            _productService = productService;
            _userService = userService;
            _httpFactory = httpFactory;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Index()
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
        public async Task<ActionResult> AddProduct(ProductDTO productModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productModel);
            }

            var user = await _userService.GetUserAsync(User);

            //product.User = user;
            productModel.UserId = user.Id;

            await _productService.AddProduct(productModel);
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
                return View(nameof(Index));
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
        public async Task<ActionResult> UpdateProduct(long productId, ProductDTO productModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productModel);
            }

            var user = await _userService.GetUserAsync(User);

            if (!productModel.UserId.Equals(user.Id))
            {
                return BadRequest("فقط سازنده محصول ، می تواند این محصول را ویرایش کند");
            }

            productModel.UserId = user.Id;

            await _productService.UpdateProductById(productId, productModel);
            return RedirectToAction(nameof(Index));
        }
    }
}
