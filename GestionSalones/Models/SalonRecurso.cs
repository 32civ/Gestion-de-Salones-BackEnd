namespace GestionSalones.Models
{
    public class SalonRecurso
    {
        public int Id { get; set; }

        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        public int RecursoId { get; set; }
        public Recursos Recurso { get; set; }
    }
}

