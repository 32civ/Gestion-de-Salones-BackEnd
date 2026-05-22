namespace GestionSalones.Models
{
    public class Docente
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public Usuarios Usuario { get; set; }
    }
}
