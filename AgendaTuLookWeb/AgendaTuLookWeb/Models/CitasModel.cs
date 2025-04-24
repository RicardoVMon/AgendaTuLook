namespace AgendaTuLookWeb.Models
{
	public class CitasModel
	{
		public long CitaId { get; set; }
		public UsuarioModel? Usuario { get; set; }
		public string? NombreCliente { get; set; }
		public ServicioModel? Servicio { get; set; }
		public string? NombreServicio { get; set; }
		public List<MetodoPagoModel>? MetodosPago { get; set; }
		public MetodoPagoModel? MetodoPago { get; set; }
		public DateTime Fecha { get; set; }
		public TimeSpan HoraInicio { get; set; }
		public TimeSpan HoraFin { get; set; }
		public long DiaTrabajoId { get; set; }
		public string? Estado { get; set; }
		public int CalificacionReview { get; set; }
		public string? ComentarioReview { get; set; }

        //--
        public double PrecioOriginal { get; set; }
		
	}
}
