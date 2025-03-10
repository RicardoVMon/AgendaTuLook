using AgendaTuLookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AgendaTuLookWeb.Controllers
{
	public class UsuariosController : Controller
	{
		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuariosController(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<IActionResult> PerfilUsuario()
        {
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Usuarios/PerfilUsuario";

                // Obtener el ID del usuario desde la sesión
                var usuarioId = HttpContext.Session.GetString("UsuarioId");

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return RedirectToAction("Login", "Autenticacion");
                }

                
                var response = await http.GetAsync($"{url}?UsuarioId={usuarioId}");

                if (response.IsSuccessStatusCode)
                {
                    var usuario = await response.Content.ReadFromJsonAsync<UsuarioModel>();
                    return View(usuario);
                }
            }

            TempData["Mensaje"] = "No se pudo obtener la información del usuario.";
            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public async Task<IActionResult> EditarPerfilUsuario()
        {
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Usuarios/PerfilUsuario";

                var usuarioId = HttpContext.Session.GetString("UsuarioId");
                if (string.IsNullOrEmpty(usuarioId))
                {
                    return RedirectToAction("Login", "Autenticacion");
                }

                var response = await http.GetAsync($"{url}?UsuarioId={usuarioId}");

                if (response.IsSuccessStatusCode)
                {
                    var usuario = await response.Content.ReadFromJsonAsync<UsuarioModel>();
                    return View(usuario);  // Retorna la vista con los datos del usuario
                }
            }

            TempData["Mensaje"] = "No se pudo cargar la información del usuario.";
            return RedirectToAction("PerfilUsuario");
        }

		[HttpPost]
		public async Task<IActionResult> EditarPerfilUsuario(UsuarioModel usuario)
		{
			if (!ModelState.IsValid)
			{
				return View(usuario);
			}

			using (var http = _httpClient.CreateClient())
			{
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Usuarios/EditarPerfilUsuario";

				var response = await http.PostAsJsonAsync(url, usuario);

				if (response.IsSuccessStatusCode)
				{
					TempData["SuccessMessage"] = "Perfil actualizado con éxito";
					return RedirectToAction("PerfilUsuario");
				}
				else
				{
					var errorMessage = await response.Content.ReadAsStringAsync();
					ModelState.AddModelError("", $"Error al actualizar el perfil: {errorMessage}");
					return View(usuario);
				}
			}
		}



	}
}