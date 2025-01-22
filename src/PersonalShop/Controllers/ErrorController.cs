using Microsoft.AspNetCore.Mvc;

namespace PersonalShop.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
