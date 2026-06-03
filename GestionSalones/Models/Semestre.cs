namespace GestionSalones.Models
{
    public class Semestre
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty; // Ej: 2026-1

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public bool EsActivo => DateTime.Now >= FechaInicio && DateTime.Now <= FechaFin;//Claudio es un genio

        
        public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
