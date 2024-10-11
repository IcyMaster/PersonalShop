using Microsoft.AspNetCore.Mvc;

namespace PersonalShop.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("AccessDenied", Name = "AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
