using System.Text;

namespace AgendaTuLookAPI.Servicios
{
	public interface ICorreos
	{
		public String GenerarCodigoVerificacion();
		public void EnviarCorreoCodigoVerificacion(string correo, string codigoVerificacion, DateTime fechaVencimiento);
		public string GenerarContenidoCorreoCodigoVerificacion(string codigo, DateTime fechaVencimiento);
	}
}
