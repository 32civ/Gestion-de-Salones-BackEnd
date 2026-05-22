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

        // ✅ GET: api/matriculas
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetMatriculas()
        {
            var matriculas = await _context.Matriculas
                .Include(m => m.Estudiante)
                    .ThenInclude(e => e.Usuario)
                .Include(m => m.Curso)
                    .ThenInclude(c => c.Materia)
                .Select(m => new
                {
                    m.Id,
                    Estudiante = m.Estudiante.Usuario.Nombre,
                    Materia = m.Curso.Materia.Nombre,
                    m.Curso.CupoMaximo
                })
                .ToListAsync<object>();

            return Ok(matriculas);
        }

        // ✅ GET: api/matriculas/mis-materias
        [HttpGet("mis-materias")]
        [Authorize(Roles = Roles.Estudiante)]
        public async Task<IActionResult> GetMisMaterias()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.UsuarioId == usuario.Id);

            if (estudiante == null)
                return NotFound("No estás registrado como estudiante");

            var matriculas = await _context.Matriculas
                .Include(m => m.Curso)
                    .ThenInclude(c => c.Materia)
                .Include(m => m.Curso)
                    .ThenInclude(c => c.Docente)
                        .ThenInclude(d => d.Usuario)
                .Where(m => m.EstudianteId == estudiante.Id)
                .Select(m => new
                {
                    m.Id,
                    Materia = m.Curso.Materia.Nombre,
                    Docente = m.Curso.Docente.Usuario.Nombre,
                    m.Curso.CupoMaximo,
                    SalonAsignado = _context.Asignaciones
                        .Where(a => a.CursoId == m.CursoId)
                        .Select(a => a.Salon.Nombre)
                        .FirstOrDefault() ?? "Sin asignar",
                    Horario = _context.Asignaciones
                        .Where(a => a.CursoId == m.CursoId)
                        .Select(a => new
                        {
                            Dia = a.Horario.DiaSemana,
                            HoraInicio = a.Horario.HoraInicio.ToString(),
                            HoraFin = a.Horario.HoraFin.ToString()
                        })
                        .FirstOrDefault()
                })
                .ToListAsync<object>();

            return Ok(matriculas);
        }

        // ✅ POST: api/matriculas
        [HttpPost]
        [Authorize(Roles = Roles.Estudiante)]
        public async Task<IActionResult> Matricularse(int cursoId)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.UsuarioId == usuario.Id);

            if (estudiante == null)
                return NotFound("No estás registrado como estudiante");

            // Verificar que el curso exista
            var curso = await _context.Cursos.FindAsync(cursoId);

            if (curso == null)
                return NotFound("Curso no encontrado");

            // Verificar que no esté ya matriculado
            var yaMatriculado = await _context.Matriculas
                .AnyAsync(m => m.EstudianteId == estudiante.Id && m.CursoId == cursoId);

            if (yaMatriculado)
                return BadRequest("Ya estás matriculado en este curso");

            // Verificar que no se haya superado el cupo máximo
            var totalMatriculados = await _context.Matriculas
                .CountAsync(m => m.CursoId == cursoId);

            if (totalMatriculados >= curso.CupoMaximo)
                return BadRequest($"El curso ya está lleno, cupo máximo: {curso.CupoMaximo}");

            var matricula = new Matricula
            {
                EstudianteId = estudiante.Id,
                CursoId = cursoId
            };

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Matrícula realizada correctamente", matricula.Id });
        }

        // ✅ DELETE: api/matriculas/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Estudiante + "," + Roles.Administrativo)]
        public async Task<IActionResult> CancelarMatricula(int id)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var matricula = await _context.Matriculas.FindAsync(id);

            if (matricula == null)
                return NotFound("Matrícula no encontrada");

            // Si es estudiante, verificar que la matrícula le pertenece
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

    }
}
