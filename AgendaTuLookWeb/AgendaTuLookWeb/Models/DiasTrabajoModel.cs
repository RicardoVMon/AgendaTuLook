namespace AgendaTuLookWeb.Models
{
	public class DiasTrabajoModel
	{
		public long DiaTrabajoId { get; set; }
		public string? NombreDia { get; set; }
		public TimeSpan HoraInicio { get; set; }
		public TimeSpan HoraFin { get; set; }
		public bool? Activo { get; set; }
	}
}
