namespace ProyectoPOO.Models
{
    public class Asignacion
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public int SalonId { get; set; }

        public int HorarioId { get; set; }

        public Horario Horario { get; set; }

        public string Estado { get; set; }
    }
}
