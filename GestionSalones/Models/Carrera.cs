namespace GestionSalones.Models
{
    public class Carrera
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Materia> Materias { get; set; } = new List<Materia>();
    }
}
