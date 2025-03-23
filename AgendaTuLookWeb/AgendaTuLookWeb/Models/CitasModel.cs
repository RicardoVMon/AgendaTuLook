namespace AgendaTuLookWeb.Models
{
	public class CitasModel
	{
		public long CitaId { get; set; }
		public long UsuarioId { get; set; }
		public long ServicioId { get; set; }
		public DateTime FechaInicio { get; set; }
		public DateTime HoraInicio { get; set; }
		public DateTime HoraFin { get; set; }
		public string? Estado { get; set; }
	}
}
