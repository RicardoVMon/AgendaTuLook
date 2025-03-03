using AgendaTuLookWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Security.Claims;

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

		[HttpPost]
		public IActionResult Login(UsuarioModel model)
		{
			model.Contrasennia = Encrypt(model.Contrasennia!);

			using (var http = _httpClient.CreateClient())
			{
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/Login";
				var response = http.PostAsJsonAsync(url, model).Result;

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var datosResult = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result.Datos!);

						HttpContext.Session.SetString("Token", datosResult!.Token!);
						HttpContext.Session.SetString("UsuarioId", datosResult!.UsuarioId.ToString()!);
						HttpContext.Session.SetString("Correo", datosResult!.Correo!.ToString()!);
						HttpContext.Session.SetString("Nombre", datosResult!.Nombre!.ToString());
						return RedirectToAction("Index", "Home");
					}

					TempData["Mensaje"] = result!.Mensaje;
					return View(model);
				}
			}
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
			model.Contrasennia = Encrypt(model.Contrasennia!);

			using (var http = _httpClient.CreateClient())
			{
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/Registro";
				var response = http.PostAsJsonAsync(url, model).Result;

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (!result!.Indicador)
					{
						TempData["Mensaje"] = result.Mensaje;
						return View(model);
					}

					return RedirectToAction("Login", "Autenticacion");

				}
			}

			return View();
		}

		[HttpPost]
		public IActionResult Logout()
		{
			using (var http = _httpClient.CreateClient())
			{
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/Logout";
				var response = http.GetAsync(url).Result;

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

		#region Google

		[HttpGet]
		public async Task<IActionResult> GoogleCallback()
		{
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			if (result?.Principal == null)
			{
				return RedirectToAction("Login", "Autenticacion");
			}

			var claims = result.Principal.Claims;
			var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
			var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			// Call the API to register/authenticate the user with Google info
			using (var http = _httpClient.CreateClient())
			{
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/LoginWithGoogle";
				var model = new UsuarioModel
				{
					Correo = email,
					Nombre = name,
					GoogleId = googleId
				};

				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var respuesta = await response.Content.ReadFromJsonAsync<RespuestaModel>();
					if (respuesta != null && respuesta.Indicador)
					{
						var datosResult = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)respuesta.Datos!);

						HttpContext.Session.SetString("Token", datosResult!.Token!);
						HttpContext.Session.SetString("UsuarioId", datosResult!.UsuarioId.ToString()!);
						HttpContext.Session.SetString("Correo", datosResult!.Correo!);
						HttpContext.Session.SetString("Nombre", datosResult!.Nombre!);

						return RedirectToAction("Index", "Home");
					}
				}
			}

			return RedirectToAction("Login", "Autenticacion");
		}

		[HttpGet]
		public async Task<IActionResult> LoginGoogle()
		{
			var properties = new AuthenticationProperties
			{
				RedirectUri = Url.Action("GoogleCallback")
			};
			return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		}

		#endregion Google

		private string Encrypt(string texto)
		{
			byte[] iv = new byte[16];
			byte[] array;

			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(_configuration.GetSection("Variables:llaveCifrado").Value!);
				aes.IV = iv;

				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(texto);
						}

						array = memoryStream.ToArray();
					}
				}
			}
			return Convert.ToBase64String(array);
		}

	}
}
