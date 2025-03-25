using System.Net.Mail;
using System.Text;

namespace AgendaTuLookAPI.Servicios
{
	public class Correos : ICorreos
	{

		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
		public Correos(IHttpClientFactory httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}

		public void EnviarCorreoCodigoVerificacion(string correo, string codigoVerificacion, DateTime fechaVencimiento)
		{
			string cuenta = _configuration["CorreoNotificaciones"]!;
			string contrasenna = _configuration["ContrasennaNotificaciones"]!;

			MailMessage message = new MailMessage();
			message.From = new MailAddress(cuenta);
			message.To.Add(new MailAddress(correo));
			message.Subject = "Código de Verificación - Recuperación de Contraseña";
			message.Body = GenerarContenidoCorreoCodigoVerificacion(codigoVerificacion, fechaVencimiento);
			message.Priority = MailPriority.Normal;
			message.IsBodyHtml = true;

			SmtpClient client = new SmtpClient("smtp.office365.com", 587);
			client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenna);
			client.EnableSsl = true;
			client.Send(message);
		}

		public string GenerarCodigoVerificacion()
		{
			const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			const int longitud = 6;

			StringBuilder codigo = new StringBuilder();
			Random rnd = new Random();

			for (int i = 0; i < longitud; i++)
			{
				codigo.Append(caracteres[rnd.Next(caracteres.Length)]);
			}

			return codigo.ToString();
		}

		public string GenerarContenidoCorreoCodigoVerificacion(string codigo, DateTime fechaVencimiento)
		{
			string contenido = $@"
			<div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 10px; background-color: #f9f9f9;'>
				<h2 style='color: #4CAF50; text-align: center;'> AgendaTuLook - Recuperación de Contraseña</h2>
				<p>¡Hola!</p>
				<p>Hemos recibido una solicitud para recuperar tu contraseña. A continuación, te proporcionamos un código de verificación:</p>
				<p style='font-size: 24px; text-align: center; letter-spacing: 5px; font-weight: bold; color: #333; padding: 10px; background-color: #eee; border-radius: 5px;'>{codigo}</p>
				<p><strong>Fecha de expiración:</strong> {fechaVencimiento:dd/MM/yyyy hh:mm tt}</p>
				<p>Ingresa este código en la página de verificación para continuar con el proceso de recuperación de contraseña.</p>
				<p style='margin-top: 30px;'>¡Gracias por usar <strong>AgendaTuLook!</strong></p>
				<hr style='margin-top: 40px;'>
				<p style='font-size: 12px; color: gray;'>Este es un mensaje automático. Por favor, no respondas a este correo.</p>
			</div>";

			return contenido;
		}
	}
}
