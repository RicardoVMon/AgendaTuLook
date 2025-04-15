namespace AgendaTuLookWeb.Models
{
    public class ReviewDestacadoModel
    {
        public long ReviewId { get; set; }
        public long CitaId { get; set; }
        public string ComentarioReview { get; set; }
        public int CalificacionReview { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
    }
}
