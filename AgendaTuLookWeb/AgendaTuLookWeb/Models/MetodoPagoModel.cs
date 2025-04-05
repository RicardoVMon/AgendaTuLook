namespace AgendaTuLookWeb.Models
{
	public class MetodoPagoModel
	{
		public long MetodoPagoId { get; set; }
		public string? Nombre { get; set; }
		public string? Descripcion { get; set; }

		// ------ Información para los posts del formulario de confirmar cita --------

		// Información de tarjeta de débito, crédito
		public string? NombreTitular { get; set; }
		public string? Direccion { get; set; }
		public string? CodigoPostal { get; set; }
		public string? Pais { get; set; }

		// Información para sinpes
		public string? Comprobante { get; set; }



	}
}
