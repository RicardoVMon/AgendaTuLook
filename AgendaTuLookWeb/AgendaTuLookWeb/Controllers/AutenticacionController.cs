using AgendaTuLookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace AgendaTuLookWeb.Controllers
{
    public class AutenticacionController : Controller
    {

		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;

		public AutenticacionController(IHttpClientFactory httpClient, IConfiguration configuration)
		{
            _httpClient = httpClient;
			_configuration = configuration;
		}

		[HttpGet]
		public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registro()
        {
			return View();
		}

        [HttpPost]
		public IActionResult Registro(UsuarioModel model)
		{
			using (var http = _httpClient.CreateClient())
			{
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/Registro";
				var response = http.PostAsJsonAsync(url, model).Result;

				if (response.IsSuccessStatusCode)
					return RedirectToAction("Login", "Autenticacion");
			}
			return View();
		}

		[HttpGet]
		public IActionResult RecuperarContrasennia()
        {
            return View();
        }
    }
}
