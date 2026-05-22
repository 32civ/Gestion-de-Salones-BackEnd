namespace GestionSalones.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public short Capacidad { get; set; }

        public ICollection<SalonRecurso> SalonRecursos { get; set; } = new List<SalonRecurso>();
    }
}
