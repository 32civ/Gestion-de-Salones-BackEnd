namespace GestionSalones.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public Usuarios Usuario { get; set; }
    }
}
