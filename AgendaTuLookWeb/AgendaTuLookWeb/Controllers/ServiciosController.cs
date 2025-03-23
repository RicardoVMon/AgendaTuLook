using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
	public class ServiciosController : Controller
	{
		public IActionResult GestionarServicios()
		{
			return View();
		}
	}
}
