using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
	public class CalendarioController : Controller
	{

		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CalendarioController(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}

		// Esta vista puede ser la misma para ambos usuarios, nada más que después se valide según el rol qué se debería de mostrar
		public IActionResult Calendario()
		{
			return View();
		}
	}
}
