namespace AgendaTuLookAPI.Models
{
    public class EstadisticasModel
    {
        public int CitasNuevas { get; set; }
        public int UsuariosNuevos { get; set; }
        public int UsuariosAtendidos { get; set; }
        public int ReviewsTotales { get; set; }
        public int ReviewsPositivasTotales { get; set; }
        public int ReviewsNegativasTotales { get; set; }
        public decimal? PorcentajeCitas { get; set; }
        public decimal? PorcentajeUsuarioNuevos { get; set; }
        public decimal? PorcentajeUsuariosAtendidos { get; set; }
        public decimal? PorcentajeTotalReviews { get; set; }
        public decimal? PorcentajeTotalReviewsPositivas { get; set; }
        public decimal? PorcentajeTotalReviewsNegativas { get; set; }

        public List<ReviewsModel>? Reviews { get; set; }

		public decimal IngresosTotales { get; set; }
		public List<ServicioModel>? Servicios { get; set; }
	}
}
