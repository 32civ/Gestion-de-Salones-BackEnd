using ProyectoPOO.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyectoPOO.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Salon> Salones { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Asignacion> Asignaciones { get; set; }
    }

    


}
