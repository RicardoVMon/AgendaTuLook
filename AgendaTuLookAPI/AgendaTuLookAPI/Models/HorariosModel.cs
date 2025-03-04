namespace AgendaTuLookAPI.Models
{
    public class HorariosModel
    {
        public long HorariosId  { get; set; }
        public string? HoraEntrada { get; set; }
        public string? HoraSalida { get; set; }
        public string? dia {  get; set; } //asi esta en al base de datos no cambiar el nombre.
        public string? Estado {  get; set; }


    }
}
