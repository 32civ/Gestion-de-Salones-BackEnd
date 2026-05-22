namespace GestionSalones.DTOs
{
    public class AsignacionListDTO
    {
        public int Id { get; set; }
        public string Curso { get; set; }
        public string Docente { get; set; }
        public string Salon { get; set; }
        public int Capacidad { get; set; }
        public int Dia { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Estado { get; set; }
    }
}
