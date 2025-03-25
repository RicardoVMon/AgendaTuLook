using AgendaTuLookAPI.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookAPI.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorController : Controller
	{
		[Route("CapturarError")]
		public IActionResult CapturarError()
		{
			var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();
			
			var respuesta = new RespuestaModel();

			respuesta.Indicador = false;
			respuesta.Mensaje = "Se presentó un problema en el sistema.";

			return Ok(respuesta);
		}
	}
}
