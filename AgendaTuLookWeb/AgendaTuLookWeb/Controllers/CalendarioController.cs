using AgendaTuLookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

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
		[HttpGet]
		public  IActionResult Calendario()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ConsultarCitasCalendario()
		{
			using (var http = _httpClient.CreateClient())
			{
				long usuarioId = Convert.ToInt64(HttpContext.Session.GetString("UsuarioId"));

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Calendario/ConsultarCitasCalendario?Id=" + usuarioId + "&c=" + 2;
				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();
					if (result != null && result.Indicador)
					{
						var citas = JsonSerializer.Deserialize<List<CitasModel>>((JsonElement)result.Datos!)!;

						var eventos = citas.Select(c => new
						{
							id = c.CitaId,
							title = c.NombreCliente ?? "Sin nombre",
							start = c.Fecha.ToString("yyyy-MM-dd") + "T" + c.HoraInicio.ToString(@"hh\:mm"),
							end = c.Fecha.ToString("yyyy-MM-dd") + "T" + c.HoraFin.ToString(@"hh\:mm"),
						}).ToList();

						return Json(eventos);
					}
				}
				return Json(new { success = false, message = "Error al obtener las citas." });
			}
		}
	}
}
