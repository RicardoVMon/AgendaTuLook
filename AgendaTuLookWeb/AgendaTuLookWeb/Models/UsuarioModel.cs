﻿namespace AgendaTuLookWeb.Models
{
	public class UsuarioModel
	{
		public long? UsuarioId { get; set; }
		public string? Nombre { get; set; }
		public string? Correo { get; set; }
		public string? Contrasennia { get; set; }
        public string? NuevaContrasennia { get; set; }
        public string? ConfirmarContrasennia { get; set; }
		public string? GoogleId { get; set; }
		public string? Telefono { get; set; }
		public string? ProveedorAuth { get; set; }
		public DateTime? FechaRegistro { get; set; }
		public long? RolId { get; set; }
		public string? Token { get; set; }
        public DateTime? FechaVencimientoVerificacion { get; set; }
		public string? CodigoVerificacion { get; set; }
		public string? RecaptchaToken { get; set; }
		public string? Imagen { get; set; }

	}
}
