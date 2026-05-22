namespace GestionSalones.Models
{
    public class Recursos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<SalonRecurso> SalonRecursos { get; set; } = new List<SalonRecurso>();
    }
}
