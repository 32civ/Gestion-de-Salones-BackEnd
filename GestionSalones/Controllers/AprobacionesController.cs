using GestionSalones.Data;
using GestionSalones.Helpers;
using GestionSalones.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionSalones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AprobacionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AprobacionesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/aprobaciones
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetAprobaciones()
        {
            var aprobaciones = await _context.AprobacionesDocente
                .Include(ad => ad.Asignacion)
                    .ThenInclude(a => a.Curso)
                        .ThenInclude(c => c.Materia)
                .Include(ad => ad.Asignacion)
                    .ThenInclude(a => a.Salon)
                .Include(ad => ad.Asignacion)
                    .ThenInclude(a => a.Horario)
                .Include(ad => ad.Docente)
                    .ThenInclude(d => d.Usuario)
                .Select(ad => new
                {
                    ad.Id,
                    Docente = ad.Docente.Usuario.Nombre,
                    Curso = ad.Asignacion.Curso.Materia.Nombre,
                    Salon = ad.Asignacion.Salon.Nombre,
                    Dia = ad.Asignacion.Horario.DiaSemana,
                    HoraInicio = ad.Asignacion.Horario.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = ad.Asignacion.Horario.HoraFin.ToString(@"hh\:mm"),
                    ad.Aprobado,
                    ad.Comentario
                })
                .ToListAsync<object>();

            return Ok(aprobaciones);
        }

        // ✅ GET: api/aprobaciones/mis-asignaciones
        // El docente ve sus propias asignaciones pendientes
        [HttpGet("mis-asignaciones")]
        [Authorize(Roles = Roles.Docente)]
        public async Task<IActionResult> GetMisAsignaciones()
        {
            // Obtener el email del docente desde el token JWT
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var docente = await _context.Docentes
                .FirstOrDefaultAsync(d => d.UsuarioId == usuario.Id);

            if (docente == null)
                return NotFound("No estás registrado como docente");

            // Buscar asignaciones de sus cursos que estén pendientes
            var asignaciones = await _context.Asignaciones
                .Include(a => a.Curso)
                    .ThenInclude(c => c.Materia)
                .Include(a => a.Salon)
                    .ThenInclude(s => s.SalonRecursos)
                        .ThenInclude(sr => sr.Recurso)
                .Include(a => a.Horario)
                .Where(a => a.Curso.DocenteId == docente.Id)
                .Select(a => new
                {
                    a.Id,
                    Materia = a.Curso.Materia.Nombre,
                    Salon = a.Salon.Nombre,
                    a.Salon.Capacidad,
                    Recursos = a.Salon.SalonRecursos
                        .Select(sr => sr.Recurso.Nombre).ToList(),
                    Dia = a.Horario.DiaSemana,
                    HoraInicio = a.Horario.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = a.Horario.HoraFin.ToString(@"hh\:mm"),
                    a.Estado
                })
                .ToListAsync<object>();

            return Ok(asignaciones);
        }

        // ✅ PUT: api/aprobaciones/5/aceptar
        [HttpPut("{asignacionId}/aceptar")]
        [Authorize(Roles = Roles.Docente)]
        public async Task<IActionResult> AceptarAsignacion(int asignacionId, string? comentario)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var docente = await _context.Docentes
                .FirstOrDefaultAsync(d => d.UsuarioId == usuario.Id);

            if (docente == null)
                return NotFound("No estás registrado como docente");

            var asignacion = await _context.Asignaciones
                .Include(a => a.Curso)
                .FirstOrDefaultAsync(a => a.Id == asignacionId);

            if (asignacion == null)
                return NotFound("Asignación no encontrada");

            // Verificar que la asignación pertenece al docente
            if (asignacion.Curso.DocenteId != docente.Id)
                return Forbid();

            if (asignacion.Estado != "Pendiente")
                return BadRequest("Solo se pueden aceptar asignaciones en estado Pendiente");

            // Actualizar estado de la asignación
            asignacion.Estado = "Aprobado";

            // Crear registro de aprobación
            var aprobacion = new AprobacionDocente
            {
                AsignacionId = asignacionId,
                DocenteId = docente.Id,
                Aprobado = true,
                Comentario = comentario ?? "Aprobado"
            };

            _context.AprobacionesDocente.Add(aprobacion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Asignación aceptada correctamente" });
        }

        // ✅ PUT: api/aprobaciones/5/rechazar
        [HttpPut("{asignacionId}/rechazar")]
        [Authorize(Roles = Roles.Docente)]
        public async Task<IActionResult> RechazarAsignacion(int asignacionId, string comentario)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var docente = await _context.Docentes
                .FirstOrDefaultAsync(d => d.UsuarioId == usuario.Id);

            if (docente == null)
                return NotFound("No estás registrado como docente");

            var asignacion = await _context.Asignaciones
                .Include(a => a.Curso)
                .FirstOrDefaultAsync(a => a.Id == asignacionId);

            if (asignacion == null)
                return NotFound("Asignación no encontrada");

            // Verificar que la asignación pertenece al docente
            if (asignacion.Curso.DocenteId != docente.Id)
                return Forbid();

            if (asignacion.Estado != "Pendiente")
                return BadRequest("Solo se pueden rechazar asignaciones en estado Pendiente");

            if (string.IsNullOrWhiteSpace(comentario))
                return BadRequest("Debe ingresar un comentario explicando el rechazo");

            // Actualizar estado
            asignacion.Estado = "Rechazado";

            // Crear registro de rechazo
            var aprobacion = new AprobacionDocente
            {
                AsignacionId = asignacionId,
                DocenteId = docente.Id,
                Aprobado = false,
                Comentario = comentario
            };

            _context.AprobacionesDocente.Add(aprobacion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Asignación rechazada", comentario });
        }

        // ✅ POST: api/aprobaciones/5/calificar
        [HttpPost("{asignacionId}/calificar")]
        [Authorize(Roles = Roles.Docente)]
        public async Task<IActionResult> CalificarSalon(int asignacionId, string comentario)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var docente = await _context.Docentes
                .FirstOrDefaultAsync(d => d.UsuarioId == usuario.Id);

            if (docente == null)
                return NotFound("No estás registrado como docente");

            var asignacion = await _context.Asignaciones
                .Include(a => a.Curso)
                .FirstOrDefaultAsync(a => a.Id == asignacionId);

            if (asignacion == null)
                return NotFound("Asignación no encontrada");

            // Verificar que la asignación pertenece al docente
            if (asignacion.Curso.DocenteId != docente.Id)
                return Forbid();

            if (asignacion.Estado != "Aprobado")
                return BadRequest("Solo se pueden calificar asignaciones aprobadas");

            if (string.IsNullOrWhiteSpace(comentario))
                return BadRequest("Debe ingresar un comentario para la calificación");

            // Verificar que no haya calificado ya
            var yacalifico = await _context.AprobacionesDocente
                .AnyAsync(ad => ad.AsignacionId == asignacionId
                             && ad.DocenteId == docente.Id
                             && ad.Aprobado == true);

            if (!yacalifico)
                return BadRequest("Solo puedes calificar una asignación que hayas aceptado");

            // Actualizar el comentario de la aprobación existente como calificación
            var aprobacion = await _context.AprobacionesDocente
                .FirstOrDefaultAsync(ad => ad.AsignacionId == asignacionId
                                        && ad.DocenteId == docente.Id);

            aprobacion.Comentario = comentario;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Salón calificado correctamente", comentario });
        }
    }
}
