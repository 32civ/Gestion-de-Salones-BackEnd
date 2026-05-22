namespace GestionSalones.DTOs
{
    public class AsignacionAutomaticaDTO
    {
        public int CursoId { get; set; }
        public int HorarioId { get; set; }
        public List<int>? RecursosRequeridos { get; set; } 
    }
}
