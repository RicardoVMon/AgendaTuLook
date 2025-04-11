using System.Text;

namespace AgendaTuLookAPI.Servicios
{
	public interface ICorreos
	{
		public String GenerarCodigoVerificacion();
		public void EnviarCorreoCodigoVerificacion(string correo, string codigoVerificacion, DateTime fechaVencimiento);
		public string GenerarContenidoCorreoCodigoVerificacion(string codigo, DateTime fechaVencimiento);

		// Función de correo para confirmación de cita
		public void EnviarCorreoFacturaCita(string correo, string nombreCliente, string nombreServicio, double precio, string metodoPago, string fecha, string horaInicio, string horaFin);
		public string GenerarContenidoCorreoFacturaCita(string nombreCliente, string nombreServicio, double precio, string metodoPago, string fecha, string horaInicio, string horaFin);
		public string GenerateICSInviteBody(
			string nombreCliente,
			string correoCliente,
			string nombreServicio,
			double precio,
			string metodoPago,
			string fecha,       // formato esperado: "yyyy-MM-dd"
			string horaInicio,  // formato esperado: "HH:mm"
			string horaFin      // formato esperado: "HH:mm"
		);
		}
}
