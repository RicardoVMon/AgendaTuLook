using AgendaTuLookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using AgendaTuLookWeb.Servicios;

namespace AgendaTuLookWeb.Controllers
{
    [FiltroSesion]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public class UsuariosController : Controller
	{
		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISeguridad _seguridad;

		public UsuariosController(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISeguridad seguridad)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
			_seguridad = seguridad;
		}
		// Este metodo y el de abajo se podrían meter en una interfaz
        [HttpGet]
        public async Task<IActionResult> PerfilUsuario(long Id)
        {
            using (var http = _httpClient.CreateClient())
            {
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Usuarios/PerfilUsuario?Id=" + Id;

                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
                        var usuario = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result.Datos!)!;
						return View(usuario);
					}
                }
            }

            TempData["ErrorMessage"] = "No se pudo obtener la información del usuario.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditarPerfilUsuario(long Id)
        {
            using (var http = _httpClient.CreateClient())
            {

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Usuarios/PerfilUsuario?Id=" + Id;

                var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var usuario = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result.Datos!)!;
						return View(usuario);
					}
				}
			}
            TempData["ErrorMessage"] = "No se pudo cargar la información del usuario.";
            return RedirectToAction("PerfilUsuario");
        }

		[HttpPost]
		public async Task<IActionResult> EditarPerfilUsuario(UsuarioModel model)
		{
            using (var http = _httpClient.CreateClient())
			{

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Usuarios/EditarPerfilUsuario";

				var response = await http.PutAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{

					HttpContext.Session.SetString("Nombre", model.Nombre!);
					TempData["SuccessMessage"] = "Perfil actualizado con éxito";
					return RedirectToAction("PerfilUsuario", "Usuarios", new { Id = model.UsuarioId});
				}
				TempData["ErrorMessage"] = "No se pudo actualizar el perfil";
				return View(model);
			}
		}

		[HttpPost]
		public async Task<IActionResult> EditarContrasenniaUsuario(UsuarioModel model)
		{
			using (var http = _httpClient.CreateClient())
			{

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Usuarios/EditarContrasenniaUsuario";

				model.Contrasennia = _seguridad.Encrypt(model.Contrasennia!);
				model.NuevaContrasennia = _seguridad.Encrypt(model.NuevaContrasennia!);
				model.ConfirmarContrasennia = _seguridad.Encrypt(model.ConfirmarContrasennia!);

				var response = await http.PutAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && !result.Indicador)
					{
						TempData["ErrorMessage"] = result.Mensaje;
						return RedirectToAction("EditarPerfilUsuario", "Usuarios", new { Id = model.UsuarioId });
					}

					TempData["SuccessMessage"] = "Contraseña actualizada con éxito";
					return RedirectToAction("PerfilUsuario", "Usuarios", new { Id = model.UsuarioId });
				}

				TempData["ErrorMessage"] = "No se pudo actualizar el perfil";
				return RedirectToAction("EditarPerfilUsuario", "Usuarios", new { Id = model.UsuarioId });
			}
		}
    }
}