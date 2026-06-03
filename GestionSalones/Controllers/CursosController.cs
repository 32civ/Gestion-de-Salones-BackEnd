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
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        // ── Helper: obtener semestre activo ───────────────────────────────
        private async Task<Semestre?> GetSemestreActivoAsync()
        {
            var ahora = DateTime.Now;
            return await _context.Semestres
                .FirstOrDefaultAsync(s => s.FechaInicio <= ahora && s.FechaFin >= ahora);
        }

        // ✅ GET: api/cursos — filtra por semestre activo
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetCursos()
        {
            var semestre = await GetSemestreActivoAsync();
            if (semestre == null)
                return NotFound("No hay un semestre activo. Contacta al administrador.");

            var cursos = await _context.Cursos
                .AsNoTracking()
                .Where(c => c.SemestreId == semestre.Id) // ← filtro por semestre
                .Select(c => new CursoListDTO
                {
                    Id = c.Id,
                    Materia = c.Materia.Nombre,
                    Docente = c.Docente.Usuario.Nombre,
                    CupoMaximo = c.CupoMaximo,
                    SalonAsignado = c.Asignaciones
                        .Select(a => a.Salon.Nombre)
                        .FirstOrDefault() ?? "Sin asignar"
                })
                .ToListAsync();

            return Ok(cursos);
        }

        // ✅ GET: api/cursos/todos — todos los cursos sin filtro de semestre (para reportes)
        [HttpGet("todos")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetTodosCursos()
        {
            var cursos = await _context.Cursos
                .AsNoTracking()
                .Select(c => new
                {
                    c.Id,
                    Materia = c.Materia.Nombre,
                    Docente = c.Docente.Usuario.Nombre,
                    c.CupoMaximo,
                    Semestre = c.Semestre.Nombre,
                    SalonAsignado = c.Asignaciones
                        .Select(a => a.Salon.Nombre)
                        .FirstOrDefault() ?? "Sin asignar"
                })
                .ToListAsync();

            return Ok(cursos);
        }

        // ✅ GET: api/cursos/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetCurso(int id)
        {
            var curso = await _context.Cursos
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CursoDetalleDTO
                {
                    Id = c.Id,
                    Materia = c.Materia.Nombre,
                    Docente = c.Docente.Usuario.Nombre,
                    CupoMaximo = c.CupoMaximo,
                    Asignacion = c.Asignaciones
                        .Select(a => new AsignacionDTO
                        {
                            Salon = a.Salon!.Nombre,
                            Capacidad = a.Salon.Capacidad,
                            Dia = a.Horario!.DiaSemana,
                            HoraInicio = a.Horario.HoraInicio.ToString(@"hh\:mm"),
                            HoraFin = a.Horario.HoraFin.ToString(@"hh\:mm"),
                            Estado = a.Estado ?? "Activo"
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (curso == null)
                return NotFound("Curso no encontrado");

            return Ok(curso);
        }

        // ✅ GET: api/cursos/5/salon
        [HttpGet("{id}/salon")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetSalonDelCurso(int id)
        {
            var existe = await _context.Cursos.AsNoTracking().AnyAsync(c => c.Id == id);
            if (!existe) return NotFound("Curso no encontrado");

            var salon = await _context.Asignaciones
                .AsNoTracking()
                .Where(a => a.CursoId == id)
                .Select(a => new
                {
                    Salon = a.Salon.Nombre,
                    a.Salon.Capacidad,
                    Recursos = a.Salon.SalonRecursos.Select(sr => sr.Recurso.Nombre).ToList(),
                    Dia = a.Horario.DiaSemana,
                    HoraInicio = a.Horario.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = a.Horario.HoraFin.ToString(@"hh\:mm"),
                    a.Estado
                })
                .FirstOrDefaultAsync();

            if (salon == null)
                return NotFound("Este curso no tiene salón asignado aún");

            return Ok(salon);
        }

        // ✅ POST: api/cursos — asigna automáticamente el semestre activo
        [HttpPost]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> CrearCurso(CrearCursoDTO dto)
        {
            if (dto.CupoMaximo <= 0)
                return BadRequest("El cupo máximo debe ser mayor a 0");

            // Verificar semestre activo
            var semestre = await GetSemestreActivoAsync();
            if (semestre == null)
                return BadRequest("No hay un semestre activo. Crea uno antes de agregar cursos.");

            var materiaExiste = await _context.Materias.AnyAsync(m => m.Id == dto.MateriaId);
            if (!materiaExiste) return NotFound("La materia especificada no existe");

            var docenteExiste = await _context.Docentes.AnyAsync(d => d.Id == dto.DocenteId);
            if (!docenteExiste) return NotFound("El docente especificado no existe");

            var curso = new Curso
            {
                MateriaId = dto.MateriaId,
                DocenteId = dto.DocenteId,
                CupoMaximo = dto.CupoMaximo,
                SemestreId = semestre.Id  // ← asigna el semestre activo automáticamente
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        // ✅ PUT: api/cursos/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> EditarCurso(int id, CrearCursoDTO dto)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound("Curso no encontrado");

            if (dto.CupoMaximo <= 0)
                return BadRequest("El cupo máximo debe ser mayor a 0");

            var materiaExiste = await _context.Materias.AnyAsync(m => m.Id == dto.MateriaId);
            if (!materiaExiste) return NotFound("La materia no existe");

            var docenteExiste = await _context.Docentes.AnyAsync(d => d.Id == dto.DocenteId);
            if (!docenteExiste) return NotFound("El docente no existe");

            // Si el docente cambió, resetear asignaciones rechazadas
            if (curso.DocenteId != dto.DocenteId)
            {
                var asignacionesRechazadas = await _context.Asignaciones
                    .Where(a => a.CursoId == id && a.Estado == "Rechazado")
                    .ToListAsync();

                foreach (var asignacion in asignacionesRechazadas)
                {
                    asignacion.Estado = "Pendiente";
                    var aprobacionAnterior = await _context.AprobacionesDocente
                        .FirstOrDefaultAsync(ap => ap.AsignacionId == asignacion.Id);
                    if (aprobacionAnterior != null)
                        _context.AprobacionesDocente.Remove(aprobacionAnterior);
                }
            }

            curso.MateriaId = dto.MateriaId;
            curso.DocenteId = dto.DocenteId;
            curso.CupoMaximo = dto.CupoMaximo;
            // SemestreId no cambia al editar

            await _context.SaveChangesAsync();
            return Ok(curso);
        }

        // ✅ DELETE: api/cursos/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> EliminarCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound("Curso no encontrado");

            var tieneAsignaciones = await _context.Asignaciones
                .AnyAsync(a => a.CursoId == id && a.Estado != "Cancelada");
            if (tieneAsignaciones)
                return BadRequest("No se puede eliminar el curso porque tiene asignaciones activas");

            var tieneMatriculas = await _context.Matriculas
                .AnyAsync(m => m.CursoId == id);
            if (tieneMatriculas)
                return BadRequest("No se puede eliminar el curso porque tiene estudiantes matriculados");

            var asignacionIds = await _context.Asignaciones
                .Where(a => a.CursoId == id).Select(a => a.Id).ToListAsync();

            if (asignacionIds.Any())
            {
                var aprobaciones = await _context.AprobacionesDocente
                    .Where(ap => asignacionIds.Contains(ap.AsignacionId)).ToListAsync();
                _context.AprobacionesDocente.RemoveRange(aprobaciones);
            }

            var asignaciones = await _context.Asignaciones
                .Where(a => a.CursoId == id).ToListAsync();
            _context.Asignaciones.RemoveRange(asignaciones);

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Curso eliminado correctamente" });
        }

        // ✅ GET: api/cursos/mis-cursos — filtra por semestre activo
        [HttpGet("mis-cursos")]
        [Authorize(Roles = Roles.Docente + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetMisCursos()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("No se pudo identificar al usuario");

            var docente = await _context.Docentes
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Usuario.Email == email);
            if (docente == null)
                return NotFound("No se encontró un perfil de docente para este usuario");

            var semestre = await GetSemestreActivoAsync();
            if (semestre == null)
                return NotFound("No hay un semestre activo en este momento");

            var cursos = await _context.Cursos
                .AsNoTracking()
                .Where(c => c.DocenteId == docente.Id && c.SemestreId == semestre.Id) // ← filtro
                .Select(c => new
                {
                    c.Id,
                    Materia = c.Materia.Nombre,
                    Carrera = c.Materia.Carrera.Nombre,
                    Semestre = c.Semestre.Nombre,
                    c.CupoMaximo,
                    EstudiantesMatriculados = _context.Matriculas.Count(m => m.CursoId == c.Id),
                    Asignacion = c.Asignaciones
                        .Select(a => new
                        {
                            Salon = a.Salon.Nombre,
                            Dia = a.Horario.DiaSemana,
                            HoraInicio = a.Horario.HoraInicio.ToString(@"hh\:mm"),
                            HoraFin = a.Horario.HoraFin.ToString(@"hh\:mm"),
                            Recursos = a.Salon.SalonRecursos.Select(sr => sr.Recurso.Nombre).ToList(),
                            a.Estado
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(cursos);
        }
    }
}