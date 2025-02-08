using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
