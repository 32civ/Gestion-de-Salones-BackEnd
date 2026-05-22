namespace GestionSalones.Controllers
{
    public class DocenteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; } = true;

    }
}
