namespace AgendaTuLookWeb.Models
{
    public class ReviewsModel
    {
        public long ReviewId { get; set; }
        public string? Nombre { get; set; }
        public string? ComentarioReview { get; set; }
        public int? CalificacionReview { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
