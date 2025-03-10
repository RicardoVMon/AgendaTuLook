namespace AgendaTuLookAPI.Models
{
	public class UsuarioModel
	{
		public long? UsuarioId { get; set; }
		public string? Nombre { get; set; }
		public string? Correo { get; set; }
		public string? Contrasennia { get; set; }
		public string? GoogleId { get; set; }
		public string? Telefono { get; set; }
		public string? ProveedorAuth { get; set; }
		public DateTime? FechaRegistro { get; set; }
		public long? RolId { get; set; }
		public string? Token { get; set; }
        public bool? TieneContrasennaTemp { get; set; }
        public DateTime? FechaVencimientoTemp { get; set; }
        public string? NuevaContrasennia { get; set; }
        public string? ConfirmarContrasennia { get; set; }

    }
}
