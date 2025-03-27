using AgendaTuLookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AgendaTuLookWeb.Controllers
{
	[FiltroSesion]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public class CitasController : Controller
    {
		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
		public CitasController(IHttpClientFactory httpClient, IConfiguration configuration) { 
			_httpClient = httpClient;
			_configuration = configuration;
		}

        public IActionResult HistorialCitas()
        {
            return View();
        }

		public IActionResult GestionarCitas()
		{
			return View();
		}

		public IActionResult CuponesCitas()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> SolicitarServiciosCita()
		{
			using (var http = _httpClient.CreateClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Servicios/GestionarServicios";
				var response = await http.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;
					if (result != null && result.Indicador)
					{
						var servicios = JsonSerializer.Deserialize<List<ServicioModel>>((JsonElement)result.Datos!)!;
						return View(servicios);
					}
				}
			}
				return View();
		}

		[HttpGet]
		public IActionResult SolicitarFechaHora()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerDiasLaborables()
		{
			using (var http = _httpClient.CreateClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "DiasTrabajo/GestionarDiasTrabajo";
				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();
					if (result != null && result.Indicador)
					{
						var diasTrabajo = JsonSerializer.Deserialize<List<DiasTrabajoModel>>((JsonElement)result.Datos!)!;

						var fullCalendarDays = diasTrabajo
						.Where(d => d.Activo == true)
						.GroupBy(d => new { d.HoraInicio, d.HoraFin }) // Agrupa por horario
						.Select(grupo => new
						{
							daysOfWeek = grupo.Select(d => MapDayNameToIndex(d.NombreDia)).ToList(), // Lista de días
							startTime = grupo.Key.HoraInicio.ToString(@"hh\:mm"),
							endTime = grupo.Key.HoraFin.ToString(@"hh\:mm")
						})
						.ToList();

						return Json(fullCalendarDays);
					}
				}
				return Json(new { success = false, message = "No se encontraron días laborables" });
			}
		}

		[HttpPost]
		public async Task<IActionResult> ConsultarHorasDisponibles(DateTime fecha)
		{
			using (var http = _httpClient.CreateClient())
			{

				CitasModel model = new CitasModel
				{
					Fecha = fecha
				};

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ConsultarHorasDisponibles";
				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var horasDisponibles = JsonSerializer.Deserialize<List<HoraModel>>((JsonElement)result.Datos!)!;
						return Json(horasDisponibles);
					}
				}
				return Json(new { success = false, message = "No fue posible obtener las horas disponibles del día" });
			}
		}

		private int MapDayNameToIndex(string? nombreDia)
		{
			return nombreDia?.ToLower() switch
			{
				"lunes" => 1,
				"martes" => 2,
				"miércoles" or "miercoles" => 3,
				"jueves" => 4,
				"viernes" => 5,
				"sábado" or "sabado" => 6,
				"domingo" => 0,
				_ => -1  // Valor inválido
			};
		}

        [HttpGet]
        public IActionResult ConfirmarCita()
        {
            return View();
        }
    }
}
