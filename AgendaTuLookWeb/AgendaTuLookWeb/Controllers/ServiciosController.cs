using AgendaTuLookWeb.Models;
using AgendaTuLookWeb.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace AgendaTuLookWeb.Controllers
{
	[FiltroSesion]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public class ServiciosController : Controller
	{
		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
		

		public ServiciosController(IHttpClientFactory httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> GestionarServicios()
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
		public IActionResult CrearServicio()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CrearServicio(ServicioModel model, IFormFile Archivos)
		{
			using (var http = _httpClient.CreateClient())
			{

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Servicios/CrearServicio";

				// Pendiente trabajar con la imagen
				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						TempData["successMessage"] = "Servicio creado exitosamente";
						return RedirectToAction("GestionarServicios", "Servicios");
					}
					TempData["errorMessage"] = result!.Mensaje;
				}
			}
			return RedirectToAction("GestionarServicios");
		}

		[HttpGet]
		public async Task<IActionResult> EditarServicio(long id)
		{
			using (var http = _httpClient.CreateClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Servicios/EditarServicio?Id=" + id;
				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var servicio = JsonSerializer.Deserialize<ServicioModel>((JsonElement)result.Datos!)!;
						return View(servicio);
					}
					TempData["errorMessage"] = result!.Mensaje;
				}
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> EditarServicio(ServicioModel model, IFormFile Archivos)
		{
			using (var http = _httpClient.CreateClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Servicios/EditarServicio";

				if (Archivos != null)
				{
					model.Imagen = Archivos.FileName;
				}
				var response = await http.PutAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						TempData["successMessage"] = "Servicio editado exitosamente";
						return View(model);
					}
					TempData["errorMessage"] = result!.Mensaje;
				}
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> CambiarEstadoServicio(long id)
		{
			using (var http = _httpClient.CreateClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Servicios/CambiarEstadoServicio?Id=" + id;
				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						TempData["successMessage"] = "Servicio desactivado exitosamente";
						return RedirectToAction("GestionarServicios", "Servicios");
					}
					TempData["errorMessage"] = result!.Mensaje;
					return RedirectToAction("GestionarServicios", "Servicios");
				}
			}
			return View();
		}
	}
}
