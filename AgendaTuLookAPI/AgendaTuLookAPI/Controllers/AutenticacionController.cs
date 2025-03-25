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
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using AgendaTuLookAPI.Servicios;

namespace AgendaTuLookAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AutenticacionController : Controller
	{

		private readonly ICorreos _correos;
		private readonly IConfiguration _configuration;
		private readonly IHttpClientFactory _httpClient;
		public AutenticacionController(IConfiguration configuration, IHttpClientFactory httpClient, ICorreos correos)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_correos = correos;
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

				var respuesta = new RespuestaModel();

				// Validar si el usuario es de google
				var resultProveedor = context.QueryFirstOrDefault("ObtenerProveedorAuthConCorreo", new { model.Correo });

				if (resultProveedor!.Nombre == "Google" && resultProveedor.TieneContrasennia == 0)
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "Te registraste previamente con google? Intenta iniciando sesión con google o restablece tu contraseña para continuar por este método";
					return Ok(respuesta);
				}

				var result = context.QueryFirstOrDefault<UsuarioModel>("Login",
					new { model.Correo, model.Contrasennia });

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
				model.ProveedorAuth = "Google";

				// Si no existe el usuario
				if (existeCorreo == 0)
				{
					string? contrasennia = null;
					string? telefono = null;

					var resultRegistro = context.QueryFirstOrDefault<int>("RegistroUsuario",
						new { model.Nombre, Correo = model.Correo, contrasennia, googleId = model.GoogleId, telefono, model.ProveedorAuth });

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
					// Validar si el usuario ya existe, es de email y no se ha logeado con google antes
					var resultProveedor = context.QueryFirstOrDefault("ObtenerProveedorAuthConCorreo", new { model.Correo });

					if (resultProveedor!.Nombre == "Email" && resultProveedor.TieneGoogleId == 0)
					{
						// Vinculamos la cuenta, esto actualiza el proveedor y agrega el google id
						var resultUpdateGoogleId = context.QueryFirstOrDefault("VincularGoogleId", new { model.GoogleId, model.Correo });
					}

					var result = context.QueryFirstOrDefault("ObtenerIdUsuarioConCorreo", new { Correo = model.Correo });
					model.UsuarioId = result!.UsuarioId;
                    model.Nombre = result.Nombre;
					model.Token = GenerarToken(result.UsuarioId, model.Correo!, model.Nombre!);
					respuesta.Indicador = true;
					respuesta.Datos = model;
				}
			}

			return Ok(respuesta);
		}

		[Authorize]
		[HttpGet]
		[Route("DesvincularGoogle")]
		public IActionResult DesvincularGoogle(String correo)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var result = context.Execute("DesvincularGoogleId", new { correo });
				if (result > 0)
				{
					respuesta.Indicador = true;
					respuesta.Mensaje = "Cuenta desvinculada correctamente";
				}
				else
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "No se pudo desvincular la cuenta, verifique que tiene una contraseña asociada antes de eliminar este método de login";
				}
				return Ok(respuesta);
			}
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

        [HttpPost]
        [Route("RecuperarContrasennia")]
        public IActionResult RecuperarContrasennia(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new RespuestaModel();
				respuesta.Mensaje = "Se ha enviado un código de verificación a al correo proporcionado. Use este código para continuar con el proceso de recuperación de contraseña.";
				var usuario = context.QueryFirstOrDefault<UsuarioModel>("ObtenerIdUsuarioConCorreo", new { Correo = model.Correo });

                if (usuario == null)
                {
                    respuesta.Indicador = false;
                    
                    return Ok(respuesta);
                }

				var codigoVerificacion = _correos.GenerarCodigoVerificacion();
				var fechaVencimiento = DateTime.Now.AddMinutes(30);

				context.Execute("GuardarCodigoVerificacion", new
				{
					UsuarioId = usuario.UsuarioId,
					CodigoVerificacion = codigoVerificacion,
					FechaVencimiento = fechaVencimiento
				});

				// Verificar el timeout cuando el correo es inválido
				_correos.EnviarCorreoCodigoVerificacion(model.Correo, codigoVerificacion, fechaVencimiento);

				respuesta.Indicador = true;
				respuesta.Mensaje = "Se ha enviado un código de verificación a al correo proporcionado. Use este código para continuar con el proceso de recuperación de contraseña.";
				return Ok(respuesta);
			}
        }

		[HttpPost]
		[Route("CodigoVerificacion")]
		public IActionResult CodigoVerificacion(UsuarioModel model)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();

				var verificacion = context.QueryFirstOrDefault<UsuarioModel>(
					"VerificarCodigoRecuperacion",
					new { Correo = model.Correo, Codigo = model.CodigoVerificacion }
				);

				if (verificacion == null)
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "El código de verificación es inválido.";
					return Ok(respuesta);
				}

				if (DateTime.Now > verificacion.FechaVencimientoVerificacion)
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "El código de verificación ha expirado.";
					return Ok(respuesta);
				}

				respuesta.Indicador = true;
				respuesta.Mensaje = "Código verificado correctamente.";
				//respuesta.Datos = new { Token = GenerarTokenRecuperacion(model.Correo) };
				return Ok(respuesta);
			}
		}

		[HttpPost]
        [Route("CambiarContrasenna")]
        public IActionResult CambiarContrasenna(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new RespuestaModel();
				var nuevaContrasennia = model.NuevaContrasennia;

				context.Execute("CambiarContrasenna", new { model.Correo, NuevaContrasennia = nuevaContrasennia });
                respuesta.Indicador = true;
                respuesta.Mensaje = "Contraseña actualizada correctamente.";
                return Ok(respuesta);
            }
        }

        private string Encrypt(string texto) //para poder encriptar la contra temp tambien
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

