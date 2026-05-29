using GestionSalones.Data;
using GestionSalones.DTOs;
using GestionSalones.Helpers;
using GestionSalones.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionSalones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsignacionesController : ControllerBase
    {

        private readonly AppDbContext _context;

        public AsignacionesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/asignaciones
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente)]
        public async Task<IActionResult> GetAsignaciones()
        {
            var asignaciones = await _context.Asignaciones
                .Include(a => a.Curso)
                    .ThenInclude(c => c.Materia)
                .Include(a => a.Curso)
                    .ThenInclude(c => c.Docente)
                        .ThenInclude(d => d.Usuario)
                .Include(a => a.Salon)
                .Include(a => a.Horario)
                .Select(a => new
                {
                    a.Id,
                    Curso = a.Curso.Materia.Nombre,
                    Docente = a.Curso.Docente.Usuario.Nombre,
                    Salon = a.Salon.Nombre,
                    a.Salon.Capacidad,
                    Dia = a.Horario.DiaSemana,
                    HoraInicio = a.Horario.HoraInicio.ToString(),
                    HoraFin = a.Horario.HoraFin.ToString(),
                    a.Estado
                })
                .ToListAsync<object>();

            return Ok(asignaciones);
        }

        // ✅ GET: api/asignaciones/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente)]
        public async Task<IActionResult> GetAsignacion(int id)
        {
            var asignacion = await _context.Asignaciones
                .Include(a => a.Curso)
                    .ThenInclude(c => c.Materia)
                .Include(a => a.Curso)
                    .ThenInclude(c => c.Docente)
                        .ThenInclude(d => d.Usuario)
                .Include(a => a.Salon)
                    .ThenInclude(s => s.SalonRecursos)
                        .ThenInclude(sr => sr.Recurso)
                .Include(a => a.Horario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asignacion == null)
                return NotFound("Asignación no encontrada");

            return Ok(new
            {
                asignacion.Id,
                Curso = new
                {
                    asignacion.Curso.Id,
                    Materia = asignacion.Curso.Materia.Nombre,
                    Docente = asignacion.Curso.Docente.Usuario.Nombre,
                    asignacion.Curso.CupoMaximo
                },
                Salon = new
                {
                    asignacion.Salon.Id,
                    asignacion.Salon.Nombre,
                    asignacion.Salon.Capacidad,
                    Recursos = asignacion.Salon.SalonRecursos
                        .Select(sr => sr.Recurso.Nombre).ToList()
                },
                Horario = new
                {
                    asignacion.Horario.DiaSemana,
                    HoraInicio = asignacion.Horario.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = asignacion.Horario.HoraFin.ToString(@"hh\:mm")
                },
                asignacion.Estado
            });
        }

        // ✅ GET: api/asignaciones/conflictos
        [HttpGet("conflictos")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetConflictos()
        {
            // Buscar salones asignados más de una vez en el mismo horario
            var conflictos = await _context.Asignaciones
                .GroupBy(a => new { a.SalonId, a.HorarioId })
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    g.Key.SalonId,
                    g.Key.HorarioId,
                    TotalConflictos = g.Count()
                })
                .ToListAsync<object>();

            if (!conflictos.Any())
                return Ok(new { message = "No hay conflictos de horario" });

            return Ok(conflictos);
        }

        // 🧠 POST: api/asignaciones/automatica
        [HttpPost("automatica")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AsignacionAutomatica(AsignacionAutomaticaDTO dto)
        {
            // 1️⃣ Verificar que el curso exista
            var curso = await _context.Cursos
                .Include(c => c.Materia)
                .Include(c => c.Docente)
                    .ThenInclude(d => d.Usuario)
                .FirstOrDefaultAsync(c => c.Id == dto.CursoId);

            if (curso == null)
                return NotFound("Curso no encontrado");

            // 2️⃣ Verificar que el horario exista
            var horario = await _context.Horarios.FindAsync(dto.HorarioId);

            if (horario == null)
                return NotFound("Horario no encontrado");

            // 3️⃣ Verificar que el curso no tenga ya una asignación
            var yaAsignado = await _context.Asignaciones
                .AnyAsync(a => a.CursoId == dto.CursoId);

            if (yaAsignado)
                return BadRequest("Este curso ya tiene un salón asignado");

            // 4️⃣ Obtener salones ocupados en ese horario
            var salonesOcupados = await _context.Asignaciones
                .Where(a => a.HorarioId == dto.HorarioId
                     && a.Estado != "Rechazado"
                     && a.Estado != "Cancelada")
                    .Select(a => a.SalonId)
                    .ToListAsync();

            // Verificar que el docente no tenga otro curso en ese mismo horario
            var docenteOcupado = await _context.Asignaciones
                .AnyAsync(a =>
                    a.HorarioId == dto.HorarioId &&
                    a.Curso.DocenteId == curso.DocenteId &&
                    a.Estado != "Cancelada" &&
                    a.Estado != "Rechazado"
                );

            if (docenteOcupado)
                return BadRequest("El docente ya tiene una clase asignada en ese horario");

            // 5️⃣ Buscar salones disponibles que cumplan capacidad y recursos
            var salonesDisponibles = await _context.Salones
                .Include(s => s.SalonRecursos)
                .Where(s =>
                    !salonesOcupados.Contains(s.Id) &&  // Libre en ese horario
                    s.Capacidad >= curso.CupoMaximo &&   // Capacidad suficiente
                    (dto.RecursosRequeridos == null ||   // Sin requisitos de recursos
                     dto.RecursosRequeridos.All(         // O que tenga todos los recursos requeridos
                         rId => s.SalonRecursos.Any(sr => sr.RecursoId == rId)
                     ))
                )
                .OrderBy(s => s.Capacidad) // 6️⃣ El más eficiente primero
                .ToListAsync();

            if (!salonesDisponibles.Any())
                return BadRequest("No hay salones disponibles que cumplan con los requisitos del curso");

            // 7️⃣ Tomar el salón más eficiente
            var mejorSalon = salonesDisponibles.First();

            // 8️⃣ Crear la asignación
            var asignacion = new Asignacion
            {
                CursoId = dto.CursoId,
                SalonId = mejorSalon.Id,
                HorarioId = dto.HorarioId,
                Estado = "Pendiente" // Pendiente de aprobación del docente
            };

            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Salón asignado automáticamente",
                Curso = curso.Materia.Nombre,
                Docente = curso.Docente.Usuario.Nombre,
                SalonAsignado = mejorSalon.Nombre,
                Capacidad = mejorSalon.Capacidad,
                CupoMaximo = curso.CupoMaximo,
                Horario = new
                {
                    horario.DiaSemana,
                    HoraInicio = horario.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = horario.HoraFin.ToString(@"hh\:mm")
                },
                Estado = "Pendiente de aprobación del docente"
            });
        }

        // ✅ POST: api/asignaciones/manual
        [HttpPost("manual")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AsignacionManual(AsignacionManualDTO dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.CursoId);
            if (curso == null)
                return NotFound("Curso no encontrado");

            var salon = await _context.Salones.FindAsync(dto.SalonId);
            if (salon == null)
                return NotFound("Salón no encontrado");

            var horario = await _context.Horarios.FindAsync(dto.HorarioId);
            if (horario == null)
                return NotFound("Horario no encontrado");

            // Verificar que el curso no tenga ya asignación
            var yaAsignado = await _context.Asignaciones
                .AnyAsync(a => a.CursoId == dto.CursoId);

            if (yaAsignado)
                return BadRequest("Este curso ya tiene un salón asignado");

            // Verificar que el salón esté libre en ese horario
            var salonOcupado = await _context.Asignaciones
                .AnyAsync(a => a.SalonId == dto.SalonId
                           && a.HorarioId == dto.HorarioId
                           && a.Estado != "Rechazado"
                           && a.Estado != "Cancelada");

            if (salonOcupado)
                return BadRequest("El salón ya está ocupado en ese horario");

            // Verificar que la capacidad sea suficiente
            if (salon.Capacidad < curso.CupoMaximo)
                return BadRequest($"El salón tiene capacidad para {salon.Capacidad} pero el curso necesita {curso.CupoMaximo}");

            // Verificar que el docente no tenga otro curso en ese mismo horario
            var docenteOcupado = await _context.Asignaciones
                .AnyAsync(a =>
                    a.HorarioId == dto.HorarioId &&
                    a.Curso.DocenteId == curso.DocenteId &&
                    a.Estado != "Cancelada"
                );

            if (docenteOcupado)
                return BadRequest("El docente ya tiene una clase asignada en ese horario");

            var asignacion = new Asignacion
            {
                CursoId = dto.CursoId,
                SalonId = dto.SalonId,
                HorarioId = dto.HorarioId,
                Estado = "Pendiente"
            };

            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Asignación manual creada correctamente",
                asignacion.Id,
                asignacion.Estado
            });
        }

        // ✅ PUT: api/asignaciones/5/cancelar
        [HttpPut("{id}/cancelar")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CancelarAsignacion(int id)
        {
            var asignacion = await _context.Asignaciones.FindAsync(id);

            if (asignacion == null)
                return NotFound("Asignación no encontrada");

            if (asignacion.Estado == "Cancelada")
                return BadRequest("La asignación ya está cancelada");

            asignacion.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Asignación cancelada correctamente" });
        }

    }
}
