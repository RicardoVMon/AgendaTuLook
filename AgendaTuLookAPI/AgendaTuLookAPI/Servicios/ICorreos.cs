using System.Text;

namespace AgendaTuLookAPI.Servicios
{
	public interface ICorreos
	{
		public String GenerarCodigoVerificacion();
		public bool EnviarCorreoCodigoVerificacion(string correo, string codigoVerificacion, DateTime fechaVencimiento);
		public string GenerarContenidoCorreoCodigoVerificacion(string codigo, DateTime fechaVencimiento);

		// Función de correo para confirmación de cita
		public bool EnviarCorreoFacturaCita(string correo, string nombreCliente, string nombreServicio, double precio, string metodoPago, string fecha, string horaInicio, string horaFin);
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
		public MemoryStream GenerarFacturaPDF(string nombreCliente, string nombreServicio, double precio, string metodoPago, string fecha, string horaInicio, string horaFin);

        public bool EnviarCorreoCancelacion(string correoDestino, string nombreCliente, string nombreServicio, string fecha, 
			string horaInicio, string horaFin, string metodoPago, decimal precio, bool aplicaReembolso);


		public bool EnviarCorreoFacturaCitaEdicion(
            string correo,
            string nombreCliente,
            string nombreServicio,
            double precio,
            string metodoPago,
            string fecha,
            string horaInicio,
            string horaFin,
            bool servicioCambiado);

        public string GenerarContenidoCorreoCitaEdicion(
            string nombreCliente,
            string nombreServicio,
            double precio,
            string metodoPago,
            string fecha,
            string horaInicio,
            string horaFin,
            bool servicioCambiado);

    }
}
