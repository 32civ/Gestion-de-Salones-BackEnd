using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSalones.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int? CarreraId { get; set; }

        public Usuarios Usuario { get; set; }

        public Carrera Carrera { get; set; }
    }
}
