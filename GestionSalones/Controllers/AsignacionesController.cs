using GestionSalones.Data;
using GestionSalones.DTOs;
using GestionSalones.Helpers;
using GestionSalones.Models;
using GestionSalones.Services;
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
        private readonly IEmailService _emailService;

        public AsignacionesController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ✅ GET: api/asignaciones
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente)]
        public async Task<IActionResult> GetAsignaciones()
        {
            var ahora = DateTime.Now;
            var semestreActivo = await _context.Semestres
                .FirstOrDefaultAsync(s => s.FechaInicio <= ahora && s.FechaFin >= ahora);

            if (semestreActivo == null)
                return NotFound("No hay un semestre activo");

            var asignaciones = await _context.Asignaciones
                .Include(a => a.Curso)
                    .ThenInclude(c => c.Materia)
                .Include(a => a.Curso)
                    .ThenInclude(c => c.Docente)
                        .ThenInclude(d => d.Usuario)
                .Include(a => a.Salon)
                .Include(a => a.Horario)
                .Where(a => a.Curso.SemestreId == semestreActivo.Id)
                .Select(a => new
                {
                    a.Id,
                    Curso = a.Curso.Materia.Nombre,
                    Docente = a.Curso.Docente.Usuario.Nombre,
                    Salon = a.Salon.Nombre,
                    a.Salon.Capacidad,
                    Dia = a.Horario.DiaSemana,
                    HoraInicio = a.Horario.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = a.Horario.HoraFin.ToString(@"hh\:mm"),
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
                .Include(a => a.Curso).ThenInclude(c => c.Materia)
                .Include(a => a.Curso).ThenInclude(c => c.Docente).ThenInclude(d => d.Usuario)
                .Include(a => a.Salon).ThenInclude(s => s.SalonRecursos).ThenInclude(sr => sr.Recurso)
                .Include(a => a.Horario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asignacion == null) return NotFound("Asignación no encontrada");

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
                    Recursos = asignacion.Salon.SalonRecursos.Select(sr => sr.Recurso.Nombre).ToList()
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
            var conflictos = await _context.Asignaciones
                .Where(a => a.Estado != "Cancelada" && a.Estado != "Rechazado")
                .GroupBy(a => new { a.SalonId, a.HorarioId })
                .Where(g => g.Count() > 1)
                .Select(g => new { g.Key.SalonId, g.Key.HorarioId, TotalConflictos = g.Count() })
                .ToListAsync<object>();

            if (!conflictos.Any()) return Ok(new { message = "No hay conflictos de horario" });
            return Ok(conflictos);
        }

        // 🧠 POST: api/asignaciones/automatica
        [HttpPost("automatica")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AsignacionAutomatica(AsignacionAutomaticaDTO dto)
        {
            var curso = await _context.Cursos
                .Include(c => c.Materia)
                .Include(c => c.Docente).ThenInclude(d => d.Usuario)
                .FirstOrDefaultAsync(c => c.Id == dto.CursoId);

            if (curso == null) return NotFound("Curso no encontrado");

            var horario = await _context.Horarios.FindAsync(dto.HorarioId);
            if (horario == null) return NotFound("Horario no encontrado");

            // ✅ Ignorar rechazadas y canceladas
            var yaAsignado = await _context.Asignaciones
                .AnyAsync(a => a.CursoId == dto.CursoId
                            && a.Estado != "Rechazado"
                            && a.Estado != "Cancelada");

            if (yaAsignado) return BadRequest("Este curso ya tiene un salón asignado");

            // ✅ Ignorar rechazadas y canceladas al buscar salones ocupados
            var salonesOcupados = await _context.Asignaciones
                .Where(a => a.HorarioId == dto.HorarioId &&
                            a.Estado != "Cancelada" &&
                            a.Estado != "Rechazado")
                .Select(a => a.SalonId)
                .ToListAsync();

            var docenteOcupado = await _context.Asignaciones
                .Include(a => a.Horario)
                .Include(a => a.Curso)
                .AnyAsync(a =>
                    a.Curso.DocenteId == curso.DocenteId &&
                    a.Horario.DiaSemana == horario.DiaSemana &&
                    a.Horario.HoraInicio < horario.HoraFin &&
                    a.Horario.HoraFin > horario.HoraInicio &&
                    a.Estado != "Rechazado" &&
                    a.Estado != "Cancelada");

            if (docenteOcupado)
                return BadRequest($"El docente ya tiene una clase asignada ese día en ese horario");

            var salonesDisponibles = await _context.Salones
                .Include(s => s.SalonRecursos)
                .Where(s =>
                    !salonesOcupados.Contains(s.Id) &&
                    s.Capacidad >= curso.CupoMaximo &&
                    (dto.RecursosRequeridos == null ||
                     dto.RecursosRequeridos.All(rId => s.SalonRecursos.Any(sr => sr.RecursoId == rId)))
                )
                .OrderBy(s => s.Capacidad)
                .ToListAsync();

            if (!salonesDisponibles.Any())
                return BadRequest("No hay salones disponibles que cumplan con los requisitos del curso");

            var mejorSalon = salonesDisponibles.First();

            var asignacion = new Asignacion
            {
                CursoId = dto.CursoId,
                SalonId = mejorSalon.Id,
                HorarioId = dto.HorarioId,
                Estado = "Pendiente"
            };

            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();

            // 📧 Enviar correo al docente (sin bloquear la respuesta si falla)
            _ = EnviarCorreoAsignacionAsync(curso, mejorSalon.Nombre, horario);

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
            var curso = await _context.Cursos
                .Include(c => c.Materia)
                .Include(c => c.Docente).ThenInclude(d => d.Usuario)
                .FirstOrDefaultAsync(c => c.Id == dto.CursoId);

            if (curso == null) return NotFound("Curso no encontrado");

            var salon = await _context.Salones.FindAsync(dto.SalonId);
            if (salon == null) return NotFound("Salón no encontrado");

            var horario = await _context.Horarios.FindAsync(dto.HorarioId);
            if (horario == null) return NotFound("Horario no encontrado");

            // ✅ Ignorar rechazadas y canceladas
            var yaAsignado = await _context.Asignaciones
                .AnyAsync(a => a.CursoId == dto.CursoId
                            && a.Estado != "Rechazado"
                            && a.Estado != "Cancelada");

            if (yaAsignado) return BadRequest("Este curso ya tiene un salón asignado");

            // ✅ Ignorar rechazadas y canceladas al verificar salón ocupado
            var salonOcupado = await _context.Asignaciones
                .AnyAsync(a => a.SalonId == dto.SalonId
                            && a.HorarioId == dto.HorarioId
                            && a.Estado != "Rechazado"
                            && a.Estado != "Cancelada");

            if (salonOcupado) return BadRequest("El salón ya está ocupado en ese horario");

            var docenteOcupado = await _context.Asignaciones
                .Include(a => a.Horario)
                .Include(a => a.Curso)
                .AnyAsync(a =>
                    a.Curso.DocenteId == curso.DocenteId &&
                    a.Horario.DiaSemana == horario.DiaSemana &&
                    a.Horario.HoraInicio < horario.HoraFin &&
                    a.Horario.HoraFin > horario.HoraInicio &&
                    a.Estado != "Rechazado" &&
                    a.Estado != "Cancelada");

            if (docenteOcupado)
                return BadRequest($"El docente ya tiene una clase asignada ese día en ese horario");

            if (salon.Capacidad < curso.CupoMaximo)
                return BadRequest($"El salón tiene capacidad para {salon.Capacidad} pero el curso necesita {curso.CupoMaximo}");

            var asignacion = new Asignacion
            {
                CursoId = dto.CursoId,
                SalonId = dto.SalonId,
                HorarioId = dto.HorarioId,
                Estado = "Pendiente"
            };

            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();

            // 📧 Enviar correo al docente (sin bloquear la respuesta si falla)
            _ = EnviarCorreoAsignacionAsync(curso, salon.Nombre, horario);

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
            if (asignacion == null) return NotFound("Asignación no encontrada");
            if (asignacion.Estado == "Cancelada") return BadRequest("La asignación ya está cancelada");

            asignacion.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Asignación cancelada correctamente" });
        }

        // ── Helper privado para enviar el correo ─────────────────────────────
        private async Task EnviarCorreoAsignacionAsync(Curso curso, string salonNombre, Horario horario)
        {
            try
            {
                var emailDocente = curso.Docente.Usuario.Email;
                var nombreDocente = curso.Docente.Usuario.Nombre;
                var materia = curso.Materia.Nombre;
                var dia = horario.DiaSemana switch
                {
                    1 => "Lunes",
                    2 => "Martes",
                    3 => "Miércoles",
                    4 => "Jueves",
                    5 => "Viernes",
                    6 => "Sábado",
                    7 => "Domingo",
                    _ => $"Día {horario.DiaSemana}"
                };

                await _emailService.EnviarAsignacionCreadaAsync(
                    emailDocente, nombreDocente, materia,
                    salonNombre, dia,
                    horario.HoraInicio.ToString(@"hh\:mm"),
                    horario.HoraFin.ToString(@"hh\:mm")
                );
            }
            catch (Exception ex)
            {
                // Log del error sin romper el flujo principal
                Console.WriteLine($"[EmailService] Error enviando correo: {ex.Message}");
            }
        }
    }
}