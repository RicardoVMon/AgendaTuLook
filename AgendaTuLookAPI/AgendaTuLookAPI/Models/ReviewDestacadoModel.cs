namespace AgendaTuLookAPI.Models
{
    public class ReviewDestacadoModel
    {
        public int ReviewId { get; set; }
        public int CitaId { get; set; }
        public string ComentarioReview { get; set; }
        public int CalificacionReview { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string FotoPerfil { get; set; }

    }
}
