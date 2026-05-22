namespace GestionSalones.Models
{
    public class AprobacionDocente
    {
        public int Id { get; set; }

        public int AsignacionId { get; set; }
        public int DocenteId { get; set; }

        public bool Aprobado { get; set; }// true = aprobado, false = rechazado
        public string Comentario { get; set; }

        public Asignacion Asignacion { get; set; }
        public Docente Docente { get; set; }
    }
}
