namespace AgendaTuLookWeb.Models
{
	public class DiasTrabajoModel
	{

		// Variables para llenar el calendario de agendar cita y obtener las horas de disponibles

		public long DiaTrabajoId { get; set; }
		public string? NombreDia { get; set; }
		public TimeSpan HoraInicio { get; set; }
		public TimeSpan HoraFin { get; set; }
		public bool? Activo { get; set; }
	}
}
