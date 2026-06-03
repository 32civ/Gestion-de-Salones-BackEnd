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
    public class MatriculasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatriculasController(AppDbContext context)
        {
            _context = context;
        }

        // ── Helper: semestre activo ────────────────────────────────────────
        private async Task<Semestre?> GetSemestreActivoAsync()
        {
            var ahora = DateTime.Now;
            return await _context.Semestres
                .FirstOrDefaultAsync(s => s.FechaInicio <= ahora && s.FechaFin >= ahora);
        }

        // ✅ GET: api/matriculas — solo del semestre activo
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetMatriculas()
        {
            var semestre = await GetSemestreActivoAsync();
            if (semestre == null)
                return NotFound("No hay un semestre activo");

            var matriculas = await _context.Matriculas
                .Where(m => m.SemestreId == semestre.Id)
                .Include(m => m.Estudiante).ThenInclude(e => e.Usuario)
                .Include(m => m.Curso).ThenInclude(c => c.Materia)
                .Select(m => new
                {
                    m.Id,
                    Estudiante = m.Estudiante.Usuario.Nombre,
                    Materia = m.Curso.Materia.Nombre,
                    m.Curso.CupoMaximo,
                    Semestre = m.Semestre.Nombre
                })
                .ToListAsync<object>();

            return Ok(matriculas);
        }

        // ✅ GET: api/matriculas/mis-materias — semestre activo
        [HttpGet("mis-materias")]
        [Authorize(Roles = Roles.Estudiante)]
        public async Task<IActionResult> GetMisMaterias()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return Unauthorized("Usuario no encontrado");

            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.UsuarioId == usuario.Id);
            if (estudiante == null) return NotFound("No estás registrado como estudiante");

            var semestre = await GetSemestreActivoAsync();
            if (semestre == null)
                return NotFound("No hay un semestre activo en este momento");

            var matriculas = await _context.Matriculas
                .Where(m => m.EstudianteId == estudiante.Id && m.SemestreId == semestre.Id)
                .Include(m => m.Curso).ThenInclude(c => c.Materia)
                .Include(m => m.Curso).ThenInclude(c => c.Docente).ThenInclude(d => d.Usuario)
                .Select(m => new
                {
                    m.Id,
                    Materia = m.Curso.Materia.Nombre,
                    Docente = m.Curso.Docente.Usuario.Nombre,
                    m.Curso.CupoMaximo,
                    Semestre = m.Semestre.Nombre,
                    SalonAsignado = _context.Asignaciones
                        .Where(a => a.CursoId == m.CursoId)
                        .Select(a => a.Salon.Nombre)
                        .FirstOrDefault() ?? "Sin asignar",
                    Horario = _context.Asignaciones
                        .Where(a => a.CursoId == m.CursoId)
                        .Select(a => new
                        {
                            Dia = a.Horario.DiaSemana,
                            HoraInicio = a.Horario.HoraInicio.ToString(@"hh\:mm"),
                            HoraFin = a.Horario.HoraFin.ToString(@"hh\:mm")
                        })
                        .FirstOrDefault()
                })
                .ToListAsync<object>();

            return Ok(matriculas);
        }

        // ✅ POST: api/matriculas — matricula en el semestre activo
        [HttpPost]
        [Authorize(Roles = Roles.Estudiante)]
        public async Task<IActionResult> Matricularse(int cursoId)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return Unauthorized("Usuario no encontrado");

            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.UsuarioId == usuario.Id);
            if (estudiante == null) return NotFound("No estás registrado como estudiante");

            var semestre = await GetSemestreActivoAsync();
            if (semestre == null)
                return BadRequest("No hay un semestre activo. No es posible matricularse ahora.");

            var curso = await _context.Cursos.FindAsync(cursoId);
            if (curso == null) return NotFound("Curso no encontrado");

            // Verificar que el curso pertenece al semestre activo
            if (curso.SemestreId != semestre.Id)
                return BadRequest("Este curso no pertenece al semestre activo");

            var cursoCancelado = await _context.Asignaciones
                .AnyAsync(a => a.CursoId == cursoId && a.Estado == "Cancelado");
            if (cursoCancelado)
                return BadRequest("Este curso ha sido cancelado y no admite matrículas");

            var yaMatriculado = await _context.Matriculas
                .AnyAsync(m => m.EstudianteId == estudiante.Id &&
                               m.CursoId == cursoId &&
                               m.SemestreId == semestre.Id);
            if (yaMatriculado)
                return BadRequest("Ya estás matriculado en este curso");

            var totalMatriculados = await _context.Matriculas
                .CountAsync(m => m.CursoId == cursoId && m.SemestreId == semestre.Id);
            if (totalMatriculados >= curso.CupoMaximo)
                return BadRequest($"El curso ya está lleno, cupo máximo: {curso.CupoMaximo}");

            var matricula = new Matricula
            {
                EstudianteId = estudiante.Id,
                CursoId = cursoId,
                SemestreId = semestre.Id  // ← asigna el semestre activo
            };

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Matrícula realizada correctamente",
                matricula.Id,
                Semestre = semestre.Nombre
            });
        }

        // ✅ DELETE: api/matriculas/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Estudiante + "," + Roles.Administrativo)]
        public async Task<IActionResult> CancelarMatricula(int id)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return Unauthorized("Usuario no encontrado");

            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null) return NotFound("Matrícula no encontrada");

            if (User.IsInRole(Roles.Estudiante))
            {
                var estudiante = await _context.Estudiantes
                    .FirstOrDefaultAsync(e => e.UsuarioId == usuario.Id);
                if (estudiante == null || matricula.EstudianteId != estudiante.Id)
                    return Forbid();
            }

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Matrícula cancelada correctamente" });
        }

        // ✅ GET: api/matriculas/cursos-disponibles — del semestre activo
        [HttpGet("cursos-disponibles")]
        [Authorize(Roles = Roles.Estudiante)]
        public async Task<IActionResult> GetCursosDisponibles()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return Unauthorized("Usuario no encontrado");

            var estudiante = await _context.Estudiantes
                .Include(e => e.Carrera)
                .FirstOrDefaultAsync(e => e.UsuarioId == usuario.Id);
            if (estudiante == null) return NotFound("No estás registrado como estudiante");

            if (estudiante.CarreraId == null)
                return BadRequest("No tienes una carrera asignada. Contacta al administrador.");

            var semestre = await GetSemestreActivoAsync();
            if (semestre == null)
                return NotFound("No hay un semestre activo en este momento");

            var cursosMatriculados = await _context.Matriculas
                .Where(m => m.EstudianteId == estudiante.Id && m.SemestreId == semestre.Id)
                .Select(m => m.CursoId)
                .ToListAsync();

            var cursosRaw = await _context.Cursos
                .AsNoTracking()
                .Where(c =>
                    c.SemestreId == semestre.Id &&                          // ← solo semestre activo
                    c.Materia.CarreraId == estudiante.CarreraId &&
                    !cursosMatriculados.Contains(c.Id) &&
                    !c.Asignaciones.Any(a => a.Estado == "Cancelado"))
                .Select(c => new
                {
                    c.Id,
                    Materia = c.Materia.Nombre,
                    Docente = c.Docente.Usuario.Nombre,
                    CupoMaximo = c.CupoMaximo,
                    Matriculados = _context.Matriculas.Count(m => m.CursoId == c.Id && m.SemestreId == semestre.Id),
                    Asignacion = c.Asignaciones
                        .Where(a => a.Estado == "Aprobado")
                        .Select(a => new
                        {
                            Salon = a.Salon.Nombre,
                            Dia = a.Horario.DiaSemana,
                            HoraInicio = a.Horario.HoraInicio.ToString(@"hh\:mm"),
                            HoraFin = a.Horario.HoraFin.ToString(@"hh\:mm")
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            var cursosDisponibles = cursosRaw.Select(c => new
            {
                c.Id,
                c.Materia,
                c.Docente,
                CupoMaximo = (short)c.CupoMaximo,
                CupoDisponible = (short)(c.CupoMaximo - c.Matriculados),
                c.Asignacion
            }).ToList();

            return Ok(new
            {
                Carrera = estudiante.Carrera.Nombre,
                Semestre = semestre.Nombre,
                Cursos = cursosDisponibles
            });
        }
    }
}