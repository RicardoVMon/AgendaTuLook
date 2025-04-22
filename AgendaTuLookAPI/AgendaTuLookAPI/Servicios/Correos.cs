using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using System.Net.Mail;
using System.Text;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Borders;
using iText.Layout.Properties;
using iText.Kernel.Colors;

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

			// Adjunto factura PDF
			var pdfFactura = GenerarFacturaPDF(nombreCliente, nombreServicio, precio, metodoPago, fecha, horaInicio, horaFin);
			message.Attachments.Add(new Attachment(pdfFactura, "Factura.pdf", "application/pdf"));

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

		public MemoryStream GenerarFacturaPDF(
	string nombreCliente,
	string nombreServicio,
	double precio,
	string metodoPago,
	string fecha,
	string horaInicio,
	string horaFin)
		{
			var stream = new MemoryStream();
			var writer = new PdfWriter(stream);
			var pdf = new PdfDocument(writer);
			var document = new Document(pdf);

			var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
			var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
			document.SetFont(font);

			// --- Encabezado Empresa ---
			document.Add(new Paragraph("AgendaTuLook S.A.")
				.SetFont(boldFont)
				.SetFontSize(18)
				.SetTextAlignment(TextAlignment.LEFT));

			document.Add(new Paragraph("Centro Comercial XYZ, Local 12, San José, Costa Rica\nTel: 2222-3333 | contacto@agendatulook.com")
				.SetFontSize(9)
				.SetTextAlignment(TextAlignment.LEFT));

			document.Add(new Paragraph($"Factura Electrónica No. {GenerarNumeroFactura()}")
				.SetFontSize(12)
				.SetFont(boldFont)
				.SetMarginTop(10));

			document.Add(new Paragraph($"Fecha de Emisión: {DateTime.Today:dd/MM/yyyy}")
				.SetFontSize(9));

			document.Add(new LineSeparator(new SolidLine()).SetMarginTop(5));

			// --- Información del Cliente y Detalles ---
			float[] infoCols = { 280, 280 };
			var infoTable = new Table(infoCols).UseAllAvailableWidth();

			infoTable.AddCell(new Cell().Add(new Paragraph("Información del Cliente").SetFont(boldFont)).SetBorder(Border.NO_BORDER));
			infoTable.AddCell(new Cell().Add(new Paragraph("Detalles de la Cita").SetFont(boldFont)).SetBorder(Border.NO_BORDER));

			infoTable.AddCell(new Cell().Add(new Paragraph($"{nombreCliente}")).SetBorder(Border.NO_BORDER));
			infoTable.AddCell(new Cell().Add(new Paragraph($"Método de Pago: {metodoPago}\nFecha: {fecha}\nHora: {horaInicio} - {horaFin}")).SetBorder(Border.NO_BORDER));
			document.Add(infoTable);
			document.Add(new Paragraph("\n"));

			// --- Tabla de Servicio ---
			var servicioTable = new Table(new float[] { 200, 200, 80, 80 }).UseAllAvailableWidth();

			servicioTable.AddHeaderCell(new Cell().Add(new Paragraph("Código").SetFont(boldFont)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
			servicioTable.AddHeaderCell(new Cell().Add(new Paragraph("Descripción").SetFont(boldFont)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
			servicioTable.AddHeaderCell(new Cell().Add(new Paragraph("Precio Unit.").SetFont(boldFont)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
			servicioTable.AddHeaderCell(new Cell().Add(new Paragraph("Total").SetFont(boldFont)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

			servicioTable.AddCell("S001");
			servicioTable.AddCell(nombreServicio);
			servicioTable.AddCell($"₡{precio:N2}");
			servicioTable.AddCell($"₡{precio:N2}");

			document.Add(servicioTable);
			document.Add(new Paragraph("\n"));

			// --- Totales calculados desde precio con IVAI ---
			double subtotal = precio / 1.13;
			double impuesto = precio - subtotal;

			var totales = new Table(new float[] { 400, 160 }).UseAllAvailableWidth();

			totales.AddCell(new Cell().Add(new Paragraph("Subtotal (sin IVA)").SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));
			totales.AddCell(new Cell().Add(new Paragraph($"₡{subtotal:N2}")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

			totales.AddCell(new Cell().Add(new Paragraph("IVA (13%)").SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));
			totales.AddCell(new Cell().Add(new Paragraph($"₡{impuesto:N2}")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

			totales.AddCell(new Cell().Add(new Paragraph("Total a Pagar").SetFont(boldFont).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));
			totales.AddCell(new Cell().Add(new Paragraph($"₡{precio:N2}")).SetFont(boldFont).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

			document.Add(totales);

			// --- Términos y agradecimiento ---
			document.Add(new Paragraph("\nTérminos y Condiciones:")
				.SetFont(boldFont)
				.SetFontSize(10));

			document.Add(new Paragraph("Los servicios agendados podrán ser reembolsados únicamente si la cancelación se realiza con al menos " +
				"24 horas de antelación a la cita programada. Cancelaciones realizadas con menos de 24 horas de anticipación no califican para reembolso. " +
				"En caso de inasistencia sin previo aviso, no se realizará ningún reembolso. " +
				"Al agendar una cita, el cliente acepta estos términos y condiciones. " + "\n" +
				"Gracias por confiar en AgendaTuLook.")
				.SetFontSize(9));

			// --- Footer ---
			document.Add(new LineSeparator(new SolidLine()).SetMarginTop(10));
			document.Add(new Paragraph("Factura generada automáticamente por AgendaTuLook - www.agendatulook.com")
				.SetFontSize(8)
				.SetTextAlignment(TextAlignment.CENTER)
				.SetMarginTop(5));

			document.Close();
			return new MemoryStream(stream.ToArray());
		}

		private string GenerarNumeroFactura()
		{
			Random random = new Random();
			return random.Next(10000000, 99999999).ToString();
		}

        public void EnviarCorreoCancelacion(string correo, string nombreCliente, string nombreServicio, string fecha, 
			string horaInicio, string horaFin, string metodoPago, decimal precio, bool aplicaReembolso)
        {

            string cuenta = _configuration["CorreoNotificaciones"]!;
            string contrasenna = _configuration["ContrasennaNotificaciones"]!;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(cuenta);
            message.To.Add(new MailAddress(correo));
            message.Subject = "Cancelación de Cita - AgendaTuLook";
            message.Body = GenerarContenidoCorreoCancelacion(nombreCliente, nombreServicio, fecha, horaInicio, horaFin,
                                                                metodoPago, precio, aplicaReembolso);
            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenna);
            client.EnableSsl = true;
            client.Send(message);
        }
        public string GenerarContenidoCorreoCancelacion(string nombreCliente, string nombreServicio, string fecha, string horaInicio, 
			string horaFin, string metodoPago, decimal precio, bool aplicaReembolso)
        {
            string mensajeReembolso = aplicaReembolso
                ? "La cancelación se realizó con al menos 24 horas de anticipación, por lo que se aplicará un reembolso dentro de los próximos 2 días hábiles al método de pago utilizado."
                : "La cancelación se realizó con menos de 24 horas de anticipación, por lo que no aplicará reembolso.";

            return $@"
			<div style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; max-width: 650px; margin: auto; padding: 0; color: #333; background-color: #f9f9f9;'>
				<div style='background-color: #2c3e50; padding: 25px; border-top-left-radius: 8px; border-top-right-radius: 8px; text-align: center;'>
					<h1 style='color: #ffffff; margin: 0; font-size: 26px; font-weight: 600;'>AgendaTuLook</h1>
					<p style='color: #e7e7e7; margin: 5px 0 0 0; font-size: 16px;'>Cancelación de Cita</p>
				</div>

				<div style='background-color: #ffffff; padding: 30px; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;'>
					<p style='margin-top: 0; font-size: 16px; line-height: 24px;'>Estimado/a <strong>{nombreCliente}</strong>,</p>
					<p style='font-size: 16px; line-height: 24px;'>Te informamos que tu cita ha sido cancelada. {mensajeReembolso}</p>

					<div style='margin: 25px 0; padding: 20px; background-color: #f5f7fa; border-left: 4px solid #3498db; border-radius: 4px;'>
						<h3 style='margin-top: 0; color: #2c3e50; font-size: 18px;'>Detalles del Servicio</h3>
						<table style='width: 100%; border-collapse: collapse; font-size: 15px;'>
							<tr>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0; width: 40%;'><strong>Servicio cancelado</strong></td>
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

					<div style='margin: 25px 0; padding: 20px; background-color: #f9f4e8; border-left: 4px solid #f39c12; border-radius: 4px;'>
						<h3 style='margin-top: 0; color: #2c3e50; font-size: 18px;'>Información de Pago</h3>
						<table style='width: 100%; border-collapse: collapse; font-size: 15px;'>
							<tr>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0; width: 40%;'><strong>Método de pago</strong></td>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'>{metodoPago}</td>
							</tr>
							<tr>
								<td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'><strong>Monto cobrado</strong></td>
								<td style='padding: 12px 15px; font-weight: bold; color: #16a085;'>{precio.ToString("C2", new System.Globalization.CultureInfo("es-CR"))}</td>
							</tr>
							<tr>
								<td style='padding: 12px 15px;'><strong>Aplica reembolso</strong></td>
								<td style='padding: 12px 15px;'>{(aplicaReembolso ? "Sí" : "No")}</td>
							</tr>
						</table>
					</div>

					<div style='margin-top: 30px; text-align: center;'>
						<a href='https://localhost:7258/Citas/GestionarCitas' style='display: inline-block; padding: 12px 25px; background-color: #3498db; color: #ffffff; text-decoration: none; font-weight: bold; border-radius: 4px;'>Gestionar mis citas</a>
					</div>
				</div>

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
        }

        public void EnviarCorreoFacturaCitaEdicion(
            string correo,
            string nombreCliente,
            string nombreServicio,
            double precio,
            string metodoPago,
            string fecha,
            string horaInicio,
            string horaFin,
            bool servicioCambiado)
        {
            string cuenta = _configuration["CorreoNotificaciones"]!;
            string contrasenna = _configuration["ContrasennaNotificaciones"]!;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(cuenta);
            message.To.Add(new MailAddress(correo));
            message.Subject = servicioCambiado ?
                "Confirmación de Cambios en Cita - Nueva Factura" :
                "Confirmación de Cambios en Cita";

            message.Body = GenerarContenidoCorreoCitaEdicion(
                nombreCliente, nombreServicio, precio, metodoPago,
                fecha, horaInicio, horaFin, servicioCambiado);

            // Generar Meeting iCalendar (siempre se envía)
            string calendarContent = GenerateICSInviteBody(
                nombreCliente, correo, nombreServicio, precio,
                metodoPago, fecha, horaInicio, horaFin);

            message.Attachments.Add(new Attachment(
                new MemoryStream(Encoding.UTF8.GetBytes(calendarContent)),
                "cita.ics", "text/calendar"));

            // Solo adjuntar factura PDF si el servicio cambió
            if (servicioCambiado)
            {
                var pdfFactura = GenerarFacturaPDF(
                    nombreCliente, nombreServicio, precio,
                    metodoPago, fecha, horaInicio, horaFin);

                message.Attachments.Add(new Attachment(
                    pdfFactura, "Factura.pdf", "application/pdf"));
            }

            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenna);
            client.EnableSsl = true;
            client.Send(message);
        }

        // Método para generar contenido de correo con información de cambios
        public string GenerarContenidoCorreoCitaEdicion(
			string nombreCliente,
			string nombreServicio,
			double precio,
			string metodoPago,
			string fecha,
			string horaInicio,
			string horaFin,
			bool servicioCambiado)
        {
            string mensajeCambioServicio = servicioCambiado
                ? @"<div style='background-color: #fff8e1; padding: 15px; border-radius: 5px; margin-bottom: 20px; border-left: 4px solid #ffc107;'>
              <h4 style='margin-top: 0; color: #ff8f00;'>¡Importante! - Servicio Cambiado</h4>
              <p>Has modificado el servicio de tu cita. La factura anterior ha sido anulada y se ha generado una nueva factura con los detalles actualizados.</p>
              <p>Si el nuevo servicio tiene un costo diferente al original, se ha procesado el ajuste correspondiente.</p>
           </div>"
                : @"<div style='background-color: #e3f2fd; padding: 15px; border-radius: 5px; margin-bottom: 20px; border-left: 4px solid #2196f3;'>
              <h4 style='margin-top: 0; color: #1565c0;'>Cambios Confirmados</h4>
              <p>Has actualizado los detalles de tu cita sin cambiar el servicio. Solo se han modificado la fecha y/o hora.</p>
           </div>";

            string contenido = $@"
    <div style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; max-width: 650px; margin: auto; padding: 0; color: #333; background-color: #f9f9f9;'>
        <!-- Header Section -->
        <div style='background-color: #2c3e50; padding: 25px; border-top-left-radius: 8px; border-top-right-radius: 8px; text-align: center;'>
            <h1 style='color: #ffffff; margin: 0; font-size: 26px; font-weight: 600;'>AgendaTuLook</h1>
            <p style='color: #e7e7e7; margin: 5px 0 0 0; font-size: 16px;'>Confirmación de Cambios en Cita</p>
        </div>
    
        <!-- Main Content -->
        <div style='background-color: #ffffff; padding: 30px; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;'>
            <p style='margin-top: 0; font-size: 16px; line-height: 24px;'>Estimado/a <strong>{nombreCliente}</strong>,</p>
            <p style='font-size: 16px; line-height: 24px;'>Los cambios en tu cita han sido confirmados. A continuación encontrarás el detalle actualizado:</p>
            
            {mensajeCambioServicio}
            
            <!-- Service Details Box -->
            <div style='margin: 25px 0; padding: 20px; background-color: #f5f7fa; border-left: 4px solid #3498db; border-radius: 4px;'>
                <h3 style='margin-top: 0; color: #2c3e50; font-size: 18px;'>Detalles Actualizados del Servicio</h3>
                <table style='width: 100%; border-collapse: collapse; font-size: 15px;'>
                    <tr>
                        <td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0; width: 40%;'><strong>Servicio</strong></td>
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
                    <tr>
                        <td style='padding: 12px 15px;'><strong>Precio total</strong></td>
                        <td style='padding: 12px 15px; font-weight: bold; color: #16a085;'>₡{precio.ToString("N2")}</td>
                    </tr>
                </table>
            </div>
            
            {(servicioCambiado ? $@"
            <div style='margin: 25px 0; padding: 20px; background-color: #f9f4e8; border-left: 4px solid #f39c12; border-radius: 4px;'>
                <h3 style='margin-top: 0; color: #2c3e50; font-size: 18px;'>Información de Pago</h3>
                <table style='width: 100%; border-collapse: collapse; font-size: 15px;'>
                    <tr>
                        <td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0; width: 40%;'><strong>Método de pago</strong></td>
                        <td style='padding: 12px 15px; border-bottom: 1px solid #e0e0e0;'>{metodoPago}</td>
                    </tr>
                    <tr>
                        <td style='padding: 12px 15px;'><strong>Total pagado</strong></td>
                        <td style='padding: 12px 15px; font-weight: bold; color: #16a085;'>₡{precio.ToString("N2")}</td>
                    </tr>
                </table>
            </div>" : "")}
            
            <p style='font-size: 16px; line-height: 24px;'>Si necesitas realizar más cambios o cancelar tu cita, contáctanos a través de nuestra plataforma o vía telefónica.</p>
            
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


    }
}
