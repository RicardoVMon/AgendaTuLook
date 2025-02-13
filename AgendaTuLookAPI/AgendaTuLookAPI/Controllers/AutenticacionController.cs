using AgendaTuLookAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AutenticacionController : Controller
	{

		private readonly IConfiguration _configuration;
		public AutenticacionController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[Route("Registro")] // Puede cambiar
		public IActionResult Registro(UsuarioModel usuario)
		{
			return Ok();
		}
	}
}
