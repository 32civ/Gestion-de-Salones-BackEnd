namespace GestionSalones.DTOs
{
    public class RegisterDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        // Roles.Admin
        // Roles.Administrativo
        // Roles.Docente
        // Roles.Estudiante
        public string Rol { get; set; } = string.Empty;
    }
}
