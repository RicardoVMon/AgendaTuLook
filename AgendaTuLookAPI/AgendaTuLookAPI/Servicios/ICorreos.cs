using System.Text;

namespace AgendaTuLookAPI.Servicios
{
	public interface ICorreos
	{
		public String GenerarCodigoVerificacion();
		public void EnviarCorreoCodigoVerificacion(string correo, string codigoVerificacion, DateTime fechaVencimiento);
		public string GenerarContenidoCorreoCodigoVerificacion(string codigo, DateTime fechaVencimiento);

		// Función de correo para confirmación de cita
		public void EnviarCorreoConfirmacionCita(string correo, string nombre, string fecha, string horaInicio, string horaFin, string direccion, string codigoPostal, string pais);
		public string GenerarContenidoCorreoConfirmacionCita(string nombre, string fecha, string horaInicio, string horaFin, string direccion, string codigoPostal, string pais);
	}
}
