namespace GestionSalones.DTOs
{
    public class AsignacionDetalleDTO
    {
        public int Id { get; set; }
        public CursoDTO Curso { get; set; }
        public SalonDTO Salon { get; set; }
        public HorarioDTO Horario { get; set; }
        public string Estado { get; set; }
    }
}
