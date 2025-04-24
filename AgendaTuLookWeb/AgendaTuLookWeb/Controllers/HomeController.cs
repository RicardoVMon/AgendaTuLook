using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AgendaTuLookWeb.Models;
using System.Text.Json;
using System.Net.Http.Headers;

namespace AgendaTuLookWeb.Controllers
{
	[FiltroSesion]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public class HomeController : Controller
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;

		public HomeController(IHttpClientFactory clientFactory, IConfiguration configuration)
		{
			_httpClient = clientFactory.CreateClient();
			_configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var token = HttpContext.Session.GetString("Token");
			var userIdString = HttpContext.Session.GetString("UsuarioId");

			if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userIdString))
				return View(new IndexModel());

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var baseUrl = _configuration.GetSection("Variables:urlWebApi").Value;

			var model = new IndexModel
			{
				ReviewsDestacados = await ObtenerInfoAPI<List<ReviewsModel>>($"{baseUrl}Estadisticas/ReviewsDestacadas"),
				ProximasCitas = await ObtenerInfoAPI<List<CitasModel>>($"{baseUrl}Calendario/ConsultarCitasCalendario?Id={userIdString}&c=2"),
				Servicios = await ObtenerInfoAPI<List<ServicioModel>>($"{baseUrl}Servicios/GestionarServicios")
			};
			return View(model);
		}

		private async Task<T?> ObtenerInfoAPI<T>(string url)
		{
			var response = await _httpClient.GetAsync(url);
			if (!response.IsSuccessStatusCode) return default;

			var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();
			if (result?.Indicador != true || result.Datos is not JsonElement jsonElement) return default;

			return JsonSerializer.Deserialize<T>(jsonElement);
		}
	}
}