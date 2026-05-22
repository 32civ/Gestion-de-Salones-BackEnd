using GestionSalones.Data;
using GestionSalones.DTOs;
using GestionSalones.Models;
using GestionSalones.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionSalones.Services
{
    public class AsignacionService : IAsignacionService
    {
        private readonly AppDbContext _context;

        public AsignacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AsignacionListDTO>> GetAll()
        {
            return await _context.Asignaciones
                .AsNoTracking()
                .Select(a => new AsignacionListDTO
                {
                    Id = a.Id,
                    Curso = a.Curso.Materia.Nombre,
                    Docente = a.Curso.Docente.Usuario.Nombre,
                    Salon = a.Salon.Nombre,
                    Capacidad = a.Salon.Capacidad,
                    Dia = a.Horario.DiaSemana,
                    HoraInicio = a.Horario.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = a.Horario.HoraFin.ToString(@"hh\:mm"),
                    Estado = a.Estado
                })
                .ToListAsync();
        }

        public async Task<AsignacionDetalleDTO?> GetById(int id)
        {
            return await _context.Asignaciones
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AsignacionDetalleDTO
                {
                    Id = a.Id,
                    Estado = a.Estado,
                    Curso = new CursoDTO
                    {
                        Id = a.Curso.Id,
                        Materia = a.Curso.Materia.Nombre,
                        Docente = a.Curso.Docente.Usuario.Nombre,
                        CupoMaximo = a.Curso.CupoMaximo
                    },
                    Salon = new SalonDTO
                    {
                        Id = a.Salon.Id,
                        Nombre = a.Salon.Nombre,
                        Capacidad = a.Salon.Capacidad,
                        Recursos = a.Salon.SalonRecursos
                            .Select(sr => sr.Recurso.Nombre).ToList()
                    },
                    Horario = new HorarioDTO
                    {
                        DiaSemana = a.Horario.DiaSemana,
                        HoraInicio = a.Horario.HoraInicio,
                        HoraFin = a.Horario.HoraFin
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<object> AsignacionAutomatica(AsignacionAutomaticaDTO dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.CursoId);
            var horario = await _context.Horarios.FindAsync(dto.HorarioId);

            if (curso == null || horario == null)
                return new { error = "Curso u horario no encontrado" };

            var yaAsignado = await _context.Asignaciones
                .AnyAsync(a => a.CursoId == dto.CursoId);

            if (yaAsignado)
                return new { error = "Curso ya asignado" };

            var salonesOcupados = await _context.Asignaciones
                .Where(a => a.HorarioId == dto.HorarioId)
                .Select(a => a.SalonId)
                .ToListAsync();

            var salon = await _context.Salones
                .Where(s => !salonesOcupados.Contains(s.Id)
                            && s.Capacidad >= curso.CupoMaximo)
                .OrderBy(s => s.Capacidad)
                .FirstOrDefaultAsync();

            if (salon == null)
                return new { error = "No hay salones disponibles" };

            var asignacion = new Asignacion
            {
                CursoId = dto.CursoId,
                SalonId = salon.Id,
                HorarioId = dto.HorarioId,
                Estado = "Pendiente"
            };

            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();

            return new
            {
                message = "Asignación automática realizada",
                salon.Nombre
            };
        }

        public async Task<object> AsignacionManual(AsignacionManualDTO dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.CursoId);
            var salon = await _context.Salones.FindAsync(dto.SalonId);
            var horario = await _context.Horarios.FindAsync(dto.HorarioId);

            if (curso == null || salon == null || horario == null)
                return new { error = "Datos inválidos" };

            if (salon.Capacidad < curso.CupoMaximo)
                return new { error = "Capacidad insuficiente" };

            var ocupada = await _context.Asignaciones
                .AnyAsync(a => a.SalonId == dto.SalonId && a.HorarioId == dto.HorarioId);

            if (ocupada)
                return new { error = "Salón ocupado" };

            var asignacion = new Asignacion
            {
                CursoId = dto.CursoId,
                SalonId = dto.SalonId,
                HorarioId = dto.HorarioId,
                Estado = "Pendiente"
            };

            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();

            return new { message = "Asignación manual creada" };
        }

        public async Task<bool> Cancelar(int id)
        {
            var asignacion = await _context.Asignaciones.FindAsync(id);

            if (asignacion == null)
                return false;

            asignacion.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
