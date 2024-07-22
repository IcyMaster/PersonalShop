using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Personal_Shop.Domain.Products.DTO;
using Personal_Shop.Interfaces;

namespace Personal_Shop.Controllers
{
    //[Route("Products")]
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

        //[Authorize]
        //[HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Index()
        {
            var client = _httpFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7140/Products");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
                return View(content);
            }
            else
            {
                return BadRequest();
            }
            //return View(await _productService.GetProducts());
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
        public async Task<ActionResult> AddProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var user = await _userService.GetUserAsync(User);

            product.User = user;
            product.UserId = user.Id;

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
        public async Task<ActionResult> UpdateProduct(long productId, ProductDTO product)
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
