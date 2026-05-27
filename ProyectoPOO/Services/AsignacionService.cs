using ProyectoPOO.Data;
using ProyectoPOO.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyectoPOO.Services
{
    public class AsignacionService
    {
        private readonly AppDbContext _context;

        public AsignacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AsignarSalon(int grupoId, int horarioId)
        {
            var grupo = await _context.Grupos.FindAsync(grupoId);
            var horario = await _context.Horarios.FindAsync(horarioId);

            var salones = await _context.Salones
                .Where(s => s.Capacidad >= grupo.CantidadEstudiantes)
                .ToListAsync();

            var disponibles = salones.Where(s =>
                !_context.Asignaciones
                    .Include(a => a.Horario)
                    .Any(a =>
                        a.SalonId == s.Id &&
                        a.Horario.DiaSemana == horario.DiaSemana &&
                        (
                            horario.HoraInicio < a.Horario.HoraFin &&
                            horario.HoraFin > a.Horario.HoraInicio
                        )
                    )
            ).ToList();

            // 🔥 SI HAY DISPONIBLES → NORMAL
            if (disponibles.Any())
            {
                var mejorSalon = disponibles
                    .OrderBy(s => s.Capacidad - grupo.CantidadEstudiantes)
                    .First();

                var asignacion = new Asignacion
                {
                    GrupoId = grupoId,
                    HorarioId = horarioId,
                    SalonId = mejorSalon.Id,
                    Estado = "Activa"
                };

                _context.Asignaciones.Add(asignacion);
                await _context.SaveChangesAsync();

                return $"Asignado al salón {mejorSalon.Nombre}";
            }

            // 💥 SI NO HAY → BUSCAR ALTERNATIVA
            var alternativa = await _context.Horarios
                .FirstOrDefaultAsync(h => h.DiaSemana == horario.DiaSemana);

            if (alternativa != null)
            {
                return $"No hay salón disponible. Intenta en otro horario: {alternativa.HoraInicio} - {alternativa.HoraFin}";
            }

            return "No hay salones disponibles ni alternativas";
        }
    }
}
