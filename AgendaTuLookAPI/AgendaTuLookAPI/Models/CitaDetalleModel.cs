namespace AgendaTuLookAPI.Models
{
    public class CitaDetalleModel
    {
        public long CitaId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string? Estado { get; set; }
        public long DiaTrabajoId { get; set; }

        // Datos del usuario
        public long UsuarioId { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }

        // Datos del servicio
        public long ServicioId { get; set; }
        public string? NombreServicio { get; set; }
        public decimal Precio { get; set; }
        public int Duracion { get; set; }

    }
}
