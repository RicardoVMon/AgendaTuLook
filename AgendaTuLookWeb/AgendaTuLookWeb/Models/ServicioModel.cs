namespace AgendaTuLookWeb.Models
{
	public class ServicioModel
	{
		public long ServicioId { get; set; }
		public string? NombreServicio { get; set; }
		public string? Descripcion { get; set; }
		public double Precio { get; set; }
		public int Duracion { get; set; }
		public string? Imagen { get; set; }
		public bool? Estado { get; set; }
		public decimal Ingresos { get; set; }
		public bool CambioServicio { get; set; }

	}
}
