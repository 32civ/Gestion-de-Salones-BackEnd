namespace GestionSalones.DTOs
{
    public class HorarioDTO
    {
        public int Id { get; set; }
        public int DiaSemana { get; set; }// lunes =1, martes=2, miercoles=3, jueves=4, viernes=5...
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
