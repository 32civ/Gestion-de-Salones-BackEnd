namespace GestionSalones.Models
{
    public class UsuarioRol
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuarios Usuario { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
