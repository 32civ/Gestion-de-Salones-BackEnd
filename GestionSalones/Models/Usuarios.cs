namespace GestionSalones.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    }
}
