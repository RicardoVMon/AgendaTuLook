using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
    public class AutenticacionController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult RecuperarContrasennia()
        {
            return View();
        }
    }
}
