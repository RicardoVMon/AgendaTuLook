using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
    public class CitasController : Controller
    {
        public IActionResult HistorialCitas()
        {
            return View();
        }

		public IActionResult GestionarCitas()
		{
			return View();
		}

		public IActionResult CuponesCitas()
		{
			return View();
		}
	}
}
