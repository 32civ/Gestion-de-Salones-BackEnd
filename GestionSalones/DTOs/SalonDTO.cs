namespace GestionSalones.DTOs
{
    public class SalonDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public short Capacidad { get; set; }
        public List<string> Recursos { get; set; }
    }
}
