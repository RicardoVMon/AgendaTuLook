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

                    if (result.TieneContrasennaTemp == true)
                    {
                        if (result.FechaVencimientoTemp < DateTime.Now)
                        {
                            respuesta.Indicador = false;
                            respuesta.Mensaje = "Su contraseña temporal ha expirado. Por favor, recupere su contraseña.";
                            return Ok(respuesta);
                        }
                        else
                        {
                            respuesta.Indicador = false;
                            respuesta.Mensaje = "Debe actualizar su contraseña antes de ingresar.";
                            return Ok(respuesta);
                        }
                    }

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

					// Validar si el usuario ya existe, es de email y no se ha logeado con google antes
					var resultProveedor = context.QueryFirstOrDefault("ObtenerProveedorAuthConCorreo", new { model.Correo });

					if (resultProveedor!.Nombre == "Email" && resultProveedor.TieneGoogleId == 0)
					{
						// Vinculamos la cuenta
						var resultUpdateGoogleId = context.QueryFirstOrDefault("ActualizarGoogleId", new { model.GoogleId, model.Correo });
					}

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

        [HttpPost]
        [Route("RecuperarContrasennia")]
        public IActionResult RecuperarContrasennia(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new RespuestaModel();

                var usuario = context.QueryFirstOrDefault<UsuarioModel>("ObtenerIdUsuarioConCorreo", new { Correo = model.Correo });

                if (usuario == null)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "El correo no está registrado.";
                    return Ok(respuesta);
                }

                if (usuario.ProveedorAuth == "Google" && string.IsNullOrEmpty(usuario.Contrasennia))
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "El correo electrónico ingresado no es válido para este proceso.";
                    return Ok(respuesta);
                }

                var contrasennaTemp = GenerarContrasennaTemporal();
                var contrasennaTempEncriptada = Encrypt(contrasennaTemp);

                var fechaVencimiento = DateTime.Now.AddMinutes(30);
                context.Execute("ActualizarContrasennaTemp", new { UsuarioId = usuario.UsuarioId, ContrasennaTemp = contrasennaTempEncriptada, TieneContrasennaTemp = true, FechaVencimientoTemp = fechaVencimiento
                });

                EnviarCorreoRecuperacion(usuario.Correo, contrasennaTemp, fechaVencimiento);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Se ha enviado una contraseña temporal a su correo. Use esta contraseña a continuación para proceder con el cambio de contraseña.";
                return Ok(respuesta);
            }
        }

        private string GenerarContrasennaTemporal()
        {
            const int longitudMin = 8;
            const string mayusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string minusculas = "abcdefghijklmnopqrstuvwxyz";
            const string numeros = "0123456789";
            const string caracteresEspeciales = "!@#$%^&*";
            const string res = mayusculas + minusculas + numeros + caracteresEspeciales;

            StringBuilder contrasenna = new StringBuilder();
            Random rnd = new Random();

            contrasenna.Append(mayusculas[rnd.Next(mayusculas.Length)]);
            contrasenna.Append(minusculas[rnd.Next(minusculas.Length)]);
            contrasenna.Append(numeros[rnd.Next(numeros.Length)]);
            contrasenna.Append(caracteresEspeciales[rnd.Next(caracteresEspeciales.Length)]);

            for (int i = contrasenna.Length; i < longitudMin; i++)
            {
                contrasenna.Append(res[rnd.Next(res.Length)]);
            }

            return new string(contrasenna.ToString().OrderBy(_ => rnd.Next()).ToArray());
        }

        private void EnviarCorreoRecuperacion(string correo, string contrasennaTemp, DateTime fechaVencimiento)
        {
            string cuenta = _configuration["CorreoNotificaciones"]!;
            string contrasenna = _configuration["ContrasennaNotificaciones"]!;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(cuenta);
            message.To.Add(new MailAddress(correo));
            message.Subject = "Recuperación de Contraseña";
            message.Body = GenerarContenidoCorreo(contrasennaTemp, fechaVencimiento);
            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenna);
            client.EnableSsl = true;
            client.Send(message);
        }

        private string GenerarContenidoCorreo(string contrasenna, DateTime fechaVencimiento)
        {

            string contenido = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 10px; background-color: #f9f9f9;'>
                    <h2 style='color: #4CAF50; text-align: center;'> AgendaTuLook - Recuperación de Contraseña</h2>
                    <p>¡Hola!</p>
                    <p>Hemos recibido una solicitud para recuperar tu contraseña. A continuación, te proporcionamos una contraseña temporal:</p>
                    <p><strong>Contraseña temporal:</strong> {contrasenna}</p>
                    <p><strong>Fecha de expiración:</strong> {fechaVencimiento:dd/MM/yyyy hh:mm tt}</p>
                    <p>Por favor, inicia sesión con esta contraseña temporal y continua el proceso para actualizarla.</p>
                    <p style='margin-top: 30px;'>¡Gracias por usar <strong>AgendaTuLook!</strong></p>
                    <hr style='margin-top: 40px;'>
                    <p style='font-size: 12px; color: gray;'>Este es un mensaje automático. Por favor, no respondas a este correo.</p>
                </div>";

            return contenido;


        }

        [HttpPost]
        [Route("CambiarContrasennaTemp")]
        public IActionResult CambiarContrasennaTemp(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new RespuestaModel();

                if (model.NuevaContrasennia != model.ConfirmarContrasennia)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Las contraseñas no coinciden.";
                    return Ok(respuesta);
                }

                var contrasennia = model.NuevaContrasennia;

                context.Execute("CambiarContrasennaTemp", new { model.Correo, NuevaContrasenna = contrasennia });

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

