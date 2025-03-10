using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
	public class EstadisticasController : Controller
	{
		public IActionResult Estadisticas()
		{
			return View();
		}
	}
}
