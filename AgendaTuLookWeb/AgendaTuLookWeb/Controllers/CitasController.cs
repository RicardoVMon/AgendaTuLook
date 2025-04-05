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

		[HttpGet]
		public async Task<IActionResult> HistorialCitas()
		{

			using (var http = _httpClient.CreateClient())
			{
				long usuarioId = Convert.ToInt64(HttpContext.Session.GetString("UsuarioId")!);
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Calendario/ConsultarCitasCalendario?Id=" + usuarioId + "&c=" + 1;

				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var datosConfirmar = JsonSerializer.Deserialize<List<CitasModel>>((JsonElement)result.Datos!)!;
						return View(datosConfirmar);
					}
				}
				return View();
			}
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

		[HttpGet]
		public async Task<IActionResult> ConfirmarCita(long s, DateTime f, TimeSpan h)
		{

			using (var http = _httpClient.CreateClient())
			{
				long usuarioId = Convert.ToInt64(HttpContext.Session.GetString("UsuarioId")!);
				
				CitasModel model = new CitasModel
				{
					Usuario = new UsuarioModel
					{
						UsuarioId = usuarioId
					},
					Servicio = new ServicioModel
					{
						ServicioId = s
					},
					Fecha = f,
					HoraInicio = h,
				};

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ConsultarDatosConfirmar";
				
				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var datosConfirmar = JsonSerializer.Deserialize<CitasModel>((JsonElement)result.Datos!)!;
						return View(datosConfirmar);
					}
				}
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmarCita(CitasModel model, IFormFile Archivos)
		{
			using (var http = _httpClient.CreateClient())
			{

				if (Archivos != null)
				{
					// acá extender para que esto sea la ruta del comprobante
					model.MetodoPago!.Comprobante = Archivos.FileName;
				}

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ConfirmarCita";

				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						TempData["SuccessMessage"] = "Cita Creada con Éxito!";
						return RedirectToAction("Calendario", "Calendario");
					}
				}
				TempData["errorMessage"] = "Ocurrió un error al crear la cita, vuelva a intentarlo más tarde";
				return RedirectToAction("SolicitarServiciosCita", "Citas");
			}
		}

		[HttpGet]
		public async Task<IActionResult> GestionarCitas()
		{

			using (var http = _httpClient.CreateClient())
			{
				long usuarioId = Convert.ToInt64(HttpContext.Session.GetString("UsuarioId")!);
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Calendario/ConsultarCitasCalendario?Id=" + usuarioId + "&c=" + 2;

				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var datosConfirmar = JsonSerializer.Deserialize<List<CitasModel>>((JsonElement)result.Datos!)!;
						return View(datosConfirmar);
					}
				}
				return View();
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
        
    }
}
