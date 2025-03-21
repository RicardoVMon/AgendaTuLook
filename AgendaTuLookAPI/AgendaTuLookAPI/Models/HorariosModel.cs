namespace AgendaTuLookAPI.Models
{
    public class HorariosModel
    {
        public long HorariosId  { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Boolean Estado {  get; set; }


    }
}
