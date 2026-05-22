namespace GestionSalones.Models
{
    public class Asignacion
    {
        public int Id { get; set; }

        public int CursoId { get; set; }
        public int SalonId { get; set; }
        public int HorarioId { get; set; }

        public string Estado { get; set; } // Pendiente, Aprobado

        public Curso Curso { get; set; }
        public Salon Salon { get; set; }
        public Horario Horario { get; set; }
    }
}
