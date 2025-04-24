using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
	public class ErrorController : Controller
	{
		public IActionResult CapturarError()
		{
			return View("Error");
		}
	}
}
