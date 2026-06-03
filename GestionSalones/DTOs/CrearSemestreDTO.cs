namespace GestionSalones.DTOs
{
    public class CrearSemestreDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
