using AgendaTuLookAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http;

namespace AgendaTuLookAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AutenticacionController : Controller
	{

		private readonly IConfiguration _configuration;
		private readonly IHttpClientFactory _httpClient;
		public AutenticacionController(IConfiguration configuration, IHttpClientFactory httpClient)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}

		[HttpPost]
		[Route("Registro")]
		public IActionResult Registro(UsuarioModel model)
		{

			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();

				var resultEmail = context.QueryFirstOrDefault<int>("ExisteCorreo", new { model.Correo });

				if (resultEmail > 0)
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "El Correo Ingresado Ya Se Encuentra Registrado, Pruebe Iniciando Sesión Con Google O Haciendo Login";
					return Ok(respuesta);
				}

				model.ProveedorAuth = "Email";
				var resultRegistro = context.Execute("RegistroUsuario", // esto devuelve id, pero siempre será mayor a 0 igual
				new { model.Nombre, model.Correo, model.Contrasennia, model.GoogleId, model.Telefono, model.ProveedorAuth });

				if (resultRegistro > 0)
				{
					respuesta.Indicador = true;
				}
				else
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "Su información no se ha registrado correctamente";
				}

				return Ok(respuesta);
			}
		}

		[HttpPost]
		[Route("Login")]
		public IActionResult Login(UsuarioModel model)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var result = context.QueryFirstOrDefault<UsuarioModel>("Login",
					new { model.Correo, model.Contrasennia });

				var respuesta = new RespuestaModel();

				if (result != null)
				{
					result.Token = GenerarToken(result.UsuarioId, result.Correo!, result.Nombre!);

					respuesta.Indicador = true;
					respuesta.Datos = result;
				}
				else
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "Su información no se ha validado correctamente";
				}

				return Ok(respuesta);
			}
		}

		[HttpPost]
		[Route("LoginWithGoogle")]
		public IActionResult LoginWithGoogle(UsuarioModel model)
		{
			var respuesta = new RespuestaModel();

			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var existeCorreo = context.QueryFirstOrDefault<int>("ExisteCorreo", new { Correo = model.Correo });

				// Si no existe el usuario
				if (existeCorreo == 0)
				{
					string proveedorAuth = "Google";
					string? contrasennia = null;
					string? telefono = null;

					var resultRegistro = context.QueryFirstOrDefault<int>("RegistroUsuario",
						new { model.Nombre, Correo = model.Correo, contrasennia, googleId = model.GoogleId, telefono, proveedorAuth });

					if (resultRegistro > 0)
					{
						model.UsuarioId = resultRegistro;
						model.Token = GenerarToken(resultRegistro, model.Correo!, model.Nombre!);
						respuesta.Indicador = true;
						respuesta.Datos = model;
					}
					else
					{
						respuesta.Indicador = false;
						respuesta.Mensaje = "Su información no se ha registrado correctamente";
					}
				}
				else
				{
					var usuarioId = context.QueryFirstOrDefault<int>("ObtenerIdUsuarioConCorreo", new { Correo = model.Correo });
					model.UsuarioId = usuarioId;
					model.Token = GenerarToken(usuarioId, model.Correo!, model.Nombre!);
					respuesta.Indicador = true;
					respuesta.Datos = model;
				}
			}

			return Ok(respuesta);
		}

		[HttpGet]
		[ApiExplorerSettings(IgnoreApi = true)]
		[Route("Logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok();
		}

		private string GenerarToken(long? Id, string Correo, string Nombre)
		{
			string SecretKey = _configuration.GetSection("Variables:llaveToken").Value!;

			List<Claim> claims = new List<Claim>();
			claims.Add(new Claim("Id", Id.ToString()!));
			claims.Add(new Claim("Correo", Correo.ToString()));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
			var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(20),
				signingCredentials: cred);

			return new JwtSecurityTokenHandler().WriteToken(token);

		}
	}
}

