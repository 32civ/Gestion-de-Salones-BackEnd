using Microsoft.EntityFrameworkCore;
using ASP_Proyecto.Models;

namespace ASP_Proyecto.Controllers.Datos
{
    public class contextoBD : DbContext
    {
        public contextoBD(DbContextOptions<contextoBD> options) : base(options)
        {
        }
        public DbSet<cita> citas { get; set; }

    }
}
