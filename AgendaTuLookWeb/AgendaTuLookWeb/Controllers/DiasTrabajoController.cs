using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using AgendaTuLookWeb.Models;
using System.Text.Json;

namespace AgendaTuLookWeb.Controllers
{
	[FiltroSesion]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public class DiasTrabajoController : Controller
	{
		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
		public DiasTrabajoController(IHttpClientFactory httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}

		public async Task<IActionResult> GestionarDiasTrabajo()
		{

			using (var http = _httpClient.CreateClient()) {
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "DiasTrabajo/GestionarDiasTrabajo";
				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var diasTrabajo = JsonSerializer.Deserialize<List<DiasTrabajoModel>>((JsonElement)result.Datos!)!;
						return View(diasTrabajo);
					}
				}

			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CambiarEstadoDiaTrabajo(long diaTrabajoId)
		{
			using (var http = _httpClient.CreateClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "DiasTrabajo/CambiarEstadoDiaTrabajo?Id=" + diaTrabajoId;
				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						return Json(new { success = true, message = "Estado cambiado exitosamente"});
					}
					TempData["errorMessage"] = result!.Mensaje;
				}
					return Json(new { success = false});
			}
		}

		[HttpPost]
		public async Task<IActionResult> ActualizarHorasDiaTrabajo(long diaTrabajoId, TimeSpan nuevaHoraInicio, TimeSpan nuevaHoraFin)
		{
			using (var http = _httpClient.CreateClient())
			{

				if (nuevaHoraFin < nuevaHoraInicio)
				{
					return Json(new { success = false, message = "La hora de inicio es menor a la hora de finalización, inténtelo nuevamente." });
				}

				DiasTrabajoModel model = new DiasTrabajoModel
				{
					DiaTrabajoId = diaTrabajoId,
					HoraInicio = nuevaHoraInicio,
					HoraFin = nuevaHoraFin
				};

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "DiasTrabajo/ActualizarHorasDiaTrabajo";
				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						return Json(new { success = true, message = "Hora actualizada exitosamente" });
					}
					TempData["errorMessage"] = result!.Mensaje;
				}
				return Json(new { success = false });
			}
		}
	}
}
