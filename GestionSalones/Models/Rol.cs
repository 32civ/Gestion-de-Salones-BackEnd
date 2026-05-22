namespace GestionSalones.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    }
}
