namespace GestionSalones.Models
{
    public class Matricula
    {
        public int Id { get; set; }

        public int EstudianteId { get; set; }
        public int CursoId { get; set; }

        public Estudiante Estudiante { get; set; }
        public Curso Curso { get; set; }
    }
}
