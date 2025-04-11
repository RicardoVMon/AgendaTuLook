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

		// Función de correo para código de verificación
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

		// Función de correo para confirmación de cita
		public void EnviarCorreoFacturaCita(string correo, string nombreCliente, string nombreServicio, double precio, string metodoPago, string fecha, string horaInicio, string horaFin)
		{
			string cuenta = _configuration["CorreoNotificaciones"]!;
			string contrasenna = _configuration["ContrasennaNotificaciones"]!;
			MailMessage message = new MailMessage();
			message.From = new MailAddress(cuenta);
			message.To.Add(new MailAddress(correo));
			message.Subject = "Confirmación de Cita - AgendaTuLook";
			message.Body = GenerarContenidoCorreoFacturaCita(nombreCliente, nombreServicio, precio, metodoPago, fecha, horaInicio, horaFin);

			// Generar Meeting iCalendar
			string calendarContent = GenerateICSInviteBody(nombreCliente, correo ,nombreServicio, precio, metodoPago, fecha, horaInicio, horaFin);
			message.Attachments.Add(new Attachment(new MemoryStream(Encoding.UTF8.GetBytes(calendarContent)), "cita.ics", "text/calendar"));

			message.Priority = MailPriority.Normal;
			message.IsBodyHtml = true;
			SmtpClient client = new SmtpClient("smtp.office365.com", 587);
			client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenna);
			client.EnableSsl = true;
			client.Send(message);
		}

		public string GenerarContenidoCorreoFacturaCita(string nombreCliente, string nombreServicio, double precio, string metodoPago, string fecha, string horaInicio, string horaFin)
		{
			string contenido = $@"
			<div style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; max-width: 650px; margin: auto; padding: 0; color: #333; background-color: #f9f9f9;'>
				<!-- Header Section -->
				<div style='background-color: #2c3e50; padding: 25px; border-top-left-radius: 8px; border-top-right-radius: 8px; text-align: center;'>
					<h1 style='color: #ffffff; margin: 0; font-size: 26px; font-weight: 600;'>AgendaTuLook</h1>
					<p style='color: #e7e7e7; margin: 5px 0 0 0; font-size: 16px;'>Comprobante de Servicio</p>
				</div>
            
				<!-- Main Content -->
				<div style='background-color: #ffffff; padding: 30px; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;'>
					<p style='margin-top: 0; font-size: 16px; line-height: 24px;'>Estimado/a <strong>{nombreCliente}</strong>,</p>
					<p style='font-size: 16px; line-height: 24px;'>Gracias por confiar en <strong>AgendaTuLook</strong>. A continuación encontrarás el detalle de tu servicio reciente:</p>
                
					<!-- Service Details Box -->
					<div style='margin: 25px 0; padding: 20px; background-color: #f5f7fa; border-left: 4px solid #3498db; border-radius: 4px;'>
						<h3 style='margin-top: 0; color: #2c3e50; font-size: 18px;'>Detalles del Servicio</h3>
						<table style='width: 100%; border-collapse: collapse; font-size: 15px;'>
							<tr>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0; width: 40%;'><strong>Servicio contratado</strong></td>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'>{nombreServicio}</td>
							</tr>
							<tr>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'><strong>Fecha de la cita</strong></td>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'>{fecha}</td>
							</tr>
							<tr>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'><strong>Horario</strong></td>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'>{horaInicio} - {horaFin}</td>
							</tr>
						</table>
					</div>
                
					<!-- Payment Details Box -->
					<div style='margin: 25px 0; padding: 20px; background-color: #f9f4e8; border-left: 4px solid #f39c12; border-radius: 4px;'>
						<h3 style='margin-top: 0; color: #2c3e50; font-size: 18px;'>Información de Pago</h3>
						<table style='width: 100%; border-collapse: collapse; font-size: 15px;'>
							<tr>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0; width: 40%;'><strong>Método de pago</strong></td>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'>{metodoPago}</td>
							</tr>
							<tr>
								<td style='padding: 12px 15px;'><strong>Total pagado</strong></td>
								<td style='padding: 12px 15px; font-weight: bold; color: #16a085;'>{precio.ToString("C2", new System.Globalization.CultureInfo("es-CR")) ?? "No especificado"}</td>
							</tr>
						</table>
					</div>
                
					<p style='font-size: 16px; line-height: 24px;'>Si necesitas reprogramar o cancelar tu cita, o tienes cualquier consulta adicional, contáctanos a través de nuestra plataforma o vía telefónica.</p>
                
					<div style='margin-top: 30px; text-align: center;'>
						<a href='https://localhost:7258/Citas/GestionarCitas' style='display: inline-block; padding: 12px 25px; background-color: #3498db; color: #ffffff; text-decoration: none; font-weight: bold; border-radius: 4px;'>Gestionar mis citas</a>
					</div>
				</div>
            
				<!-- Footer Section -->
				<div style='background-color: #f2f2f2; padding: 20px; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px; border: 1px solid #e0e0e0; border-top: none;'>
					<p style='text-align: center; margin: 0 0 10px 0; font-size: 14px;'>¡Gracias por elegirnos! Te esperamos pronto.</p>
					<div style='text-align: center; margin-bottom: 15px;'>
						<a href='#' style='display: inline-block; margin: 0 10px; color: #3498db; text-decoration: none;'>Facebook</a>
						<a href='#' style='display: inline-block; margin: 0 10px; color: #3498db; text-decoration: none;'>Instagram</a>
						<a href='#' style='display: inline-block; margin: 0 10px; color: #3498db; text-decoration: none;'>WhatsApp</a>
					</div>
					<p style='text-align: center; margin: 15px 0 0 0; font-size: 12px; color: #777;'>Este correo ha sido generado automáticamente. Por favor, no respondas a este mensaje.</p>
					<p style='text-align: center; margin: 5px 0 0 0; font-size: 12px; color: #777;'>© 2025 AgendaTuLook. Todos los derechos reservados.</p>
				</div>
			</div>";
			return contenido;
		}

		public string GenerateICSInviteBody(string nombreCliente, string correoCliente, string nombreServicio, double precio, string metodoPago, string fecha, string horaInicio, string horaFin)
		{
			StringBuilder str = new StringBuilder();

			// Parseo de fecha y hora
			DateTime fechaBase = DateTime.ParseExact(fecha, "dd/MM/yyyy", null);
			TimeSpan inicio = TimeSpan.Parse(horaInicio);
			TimeSpan fin = TimeSpan.Parse(horaFin);

			DateTime startDateTime = fechaBase.Add(inicio);
			DateTime endDateTime = fechaBase.Add(fin);

			str.AppendLine("BEGIN:VCALENDAR");
			str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");
			str.AppendLine("VERSION:2.0");
			str.AppendLine("METHOD:REQUEST");
			str.AppendLine("BEGIN:VEVENT");

			str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startDateTime.ToUniversalTime()));
			str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
			str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endDateTime.ToUniversalTime()));

			str.AppendLine("LOCATION:Local del servicio");
			str.AppendLine("UID:" + Guid.NewGuid().ToString());

			string descripcion = $"Cliente: {nombreCliente}\\nServicio: {nombreServicio}\\nPrecio: {precio.ToString("C2", new System.Globalization.CultureInfo("es-CR"))}\\nMétodo de Pago: {metodoPago}";
			str.AppendLine(string.Format("DESCRIPTION:{0}", descripcion.Replace("\n", "<br>")));
			str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descripcion.Replace("\n", "<br>")));
			str.AppendLine(string.Format("SUMMARY:{0} con {1}", nombreServicio, nombreCliente));

			str.AppendLine(string.Format("ORGANIZER;CN=\"{0}\":MAILTO:{1}", "AgendaTuLook", "no-reply@atl.com"));
			str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\":MAILTO:{1}", nombreCliente, correoCliente));

			str.AppendLine("BEGIN:VALARM");
			str.AppendLine("TRIGGER:-PT1H");
			str.AppendLine("ACTION:DISPLAY");
			str.AppendLine("DESCRIPTION:Recordatorio de cita");
			str.AppendLine("END:VALARM");

			str.AppendLine("END:VEVENT");
			str.AppendLine("END:VCALENDAR");

			return str.ToString();
		}
	}
}
