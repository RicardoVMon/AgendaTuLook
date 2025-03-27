namespace AgendaTuLookAPI.Models
{
	public class CitaModel
	{
		public long CitaId { get; set; }
		public long UsuarioId { get; set; }
		public long ServicioId { get; set; }
		public DateTime Fecha { get; set; }
		public TimeSpan HoraInicio { get; set; }
		public TimeSpan HoraFin { get; set; }
		public string? Estado { get; set; }
	}
}
