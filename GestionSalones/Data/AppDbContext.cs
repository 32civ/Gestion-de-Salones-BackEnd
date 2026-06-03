using GestionSalones.Models;
using Microsoft.EntityFrameworkCore;   

namespace GestionSalones.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 🔐 Seguridad
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }

        // 👨‍🏫 Académico
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }

        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Materia> Materias { get; set; }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Semestre> Semestres { get; set; }

        // 🏫 Infraestructura
        public DbSet<Salon> Salones { get; set; }
        public DbSet<Recursos> Recursos { get; set; }
        public DbSet<SalonRecurso> SalonRecursos { get; set; }

        // ⏰ Horarios y asignación
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Asignacion> Asignaciones { get; set; }

        public DbSet<AprobacionDocente> AprobacionesDocente { get; set; }

        // 🎓 Matrícula
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔐 Usuario
            modelBuilder.Entity<Usuarios>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuarios>()
                .Property(u => u.Activo)
                .HasDefaultValue(true);

            // 🔗 Usuario - Rol (Muchos a muchos)
            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRoles)
                .HasForeignKey(ur => ur.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuarioRoles)
                .HasForeignKey(ur => ur.RolId)
                .OnDelete(DeleteBehavior.Cascade);

            // 👨‍🏫 Docente (1 a 1 con Usuario)
            modelBuilder.Entity<Docente>()
                .HasOne(d => d.Usuario)
                .WithOne()
                .HasForeignKey<Docente>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // 👨‍🎓 Estudiante (1 a 1 con Usuario)
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.Usuario)
                .WithOne()
                .HasForeignKey<Estudiante>(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🎓 Materia - Carrera
            modelBuilder.Entity<Materia>()
                .HasOne(m => m.Carrera)
                .WithMany(c => c.Materias)
                .HasForeignKey(m => m.CarreraId);

            // 📚 Curso
            modelBuilder.Entity<Curso>()
                .HasOne(c => c.Docente)
                .WithMany()
                .HasForeignKey(c => c.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Curso>()
                .HasOne(c => c.Materia)
                .WithMany()
                .HasForeignKey(c => c.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🏫 Salon - Recursos (Muchos a muchos)
            modelBuilder.Entity<SalonRecurso>()
                .HasOne(sr => sr.Salon)
                .WithMany(s => s.SalonRecursos)
                .HasForeignKey(sr => sr.SalonId);

            modelBuilder.Entity<SalonRecurso>()
                .HasOne(sr => sr.Recurso)
                .WithMany(r => r.SalonRecursos)
                .HasForeignKey(sr => sr.RecursoId);

            // 🧩 Asignaciones
            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Curso)
                .WithMany(c => c.Asignaciones)
                .HasForeignKey(a => a.CursoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Salon)
                .WithMany()
                .HasForeignKey(a => a.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Horario)
                .WithMany()
                .HasForeignKey(a => a.HorarioId)
                .OnDelete(DeleteBehavior.Restrict);


            // ✅ Aprobación docente
            modelBuilder.Entity<AprobacionDocente>()
                .HasOne(ad => ad.Asignacion)
                .WithMany()
                .HasForeignKey(ad => ad.AsignacionId);

            modelBuilder.Entity<AprobacionDocente>()
                .HasOne(ad => ad.Docente)
                .WithMany()
                .HasForeignKey(ad => ad.DocenteId);

            // 🎓 Matrícula (Muchos a muchos)
            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Estudiante)
                .WithMany()
                .HasForeignKey(m => m.EstudianteId);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Curso)
                .WithMany()
                .HasForeignKey(m => m.CursoId);

            // 🔥 Restricción importante (evitar duplicados en matrícula)
            modelBuilder.Entity<Matricula>()
                .HasIndex(m => new { m.EstudianteId, m.CursoId })
                .IsUnique();

            // 🔥 Evitar duplicar recursos en el mismo salón
            modelBuilder.Entity<SalonRecurso>()
                .HasIndex(sr => new { sr.SalonId, sr.RecursoId })
                .IsUnique();

            // 📅 Semestre
            modelBuilder.Entity<Semestre>()
                .HasIndex(s => s.Nombre)
                .IsUnique();

            modelBuilder.Entity<Curso>()
                .HasOne(c => c.Semestre)
                .WithMany(s => s.Cursos)
                .HasForeignKey(c => c.SemestreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Semestre)
                .WithMany(s => s.Matriculas)
                .HasForeignKey(m => m.SemestreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Actualizar el indice único de Matricula para incluir semestre
            // (reemplaza el índice anterior que solo tenía EstudianteId + CursoId)
            modelBuilder.Entity<Matricula>()
                .HasIndex(m => new { m.EstudianteId, m.CursoId, m.SemestreId })
                .IsUnique();
        }
    }
}
