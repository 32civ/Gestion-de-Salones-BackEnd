namespace GestionSalones.Models
{
    public class Curso
    {
        public int Id { get; set; }

        public int MateriaId { get; set; }
        public int DocenteId { get; set; }

        public short CupoMaximo { get; set; }

        public Docente Docente { get; set; }
        public Materia Materia { get; set; }
        public ICollection<Asignacion> Asignaciones { get; set; }
    }
}
