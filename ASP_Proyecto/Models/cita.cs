using System.ComponentModel.DataAnnotations;

namespace ASP_Proyecto.Models
{
    public class cita
    {
        [Key] public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora")]
        public DateTime Hora { get; set; }

    }
}
