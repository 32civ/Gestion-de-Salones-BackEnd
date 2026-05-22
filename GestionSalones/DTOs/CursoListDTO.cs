namespace GestionSalones.DTOs
{
    public class CursoListDTO
    {
        public int Id { get; set; }
        public string Materia { get; set; }
        public string Docente { get; set; }
        public int CupoMaximo { get; set; }
        public string SalonAsignado { get; set; }
    }
}
