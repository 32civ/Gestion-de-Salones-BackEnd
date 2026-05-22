namespace GestionSalones.DTOs
{
    public class CursoDetalleDTO
    {
        public int Id { get; set; }
        public string Materia { get; set; }
        public string Docente { get; set; }
        public int CupoMaximo { get; set; }
        public AsignacionDTO Asignacion { get; set; }
    }
}
