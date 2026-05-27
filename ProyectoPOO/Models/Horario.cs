namespace ProyectoPOO.Models
{
    public class Horario
    {
        public int Id { get; set; }
        public string DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
