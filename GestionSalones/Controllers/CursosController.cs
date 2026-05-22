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

        // ✅ GET: api/cursos
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetCursos()
        {
            var cursos = await _context.Cursos
                .AsNoTracking()
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
            var existe = await _context.Cursos
                .AsNoTracking()
                .AnyAsync(c => c.Id == id);

            if (!existe)
                return NotFound("Curso no encontrado");

            var salon = await _context.Asignaciones
                .AsNoTracking()
                .Where(a => a.CursoId == id)
                .Select(a => new
                {
                    Salon = a.Salon.Nombre,
                    a.Salon.Capacidad,
                    Recursos = a.Salon.SalonRecursos
                        .Select(sr => sr.Recurso.Nombre).ToList(),
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

        // ✅ POST: api/cursos
        [HttpPost]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> CrearCurso(CrearCursoDTO dto)
        {
            if (dto.CupoMaximo <= 0)
                return BadRequest("El cupo máximo debe ser mayor a 0");

            var materiaExiste = await _context.Materias
                .AnyAsync(m => m.Id == dto.MateriaId);

            if (!materiaExiste)
                return NotFound("La materia especificada no existe");

            var docenteExiste = await _context.Docentes
                .AnyAsync(d => d.Id == dto.DocenteId);

            if (!docenteExiste)
                return NotFound("El docente especificado no existe");

            var duplicado = await _context.Cursos
                .AnyAsync(c => c.MateriaId == dto.MateriaId &&
                               c.DocenteId == dto.DocenteId);

            if (duplicado)
                return BadRequest("Ya existe un curso con esa materia y docente");

            var curso = new Curso
            {
                MateriaId = dto.MateriaId,
                DocenteId = dto.DocenteId,
                CupoMaximo = dto.CupoMaximo
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        // ✅ PUT: api/cursos/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> EditarCurso(int id, CrearCursoDTO dto)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
                return NotFound("Curso no encontrado");

            if (dto.CupoMaximo <= 0)
                return BadRequest("El cupo máximo debe ser mayor a 0");

            var materiaExiste = await _context.Materias
                .AnyAsync(m => m.Id == dto.MateriaId);

            if (!materiaExiste)
                return NotFound("La materia no existe");

            var docenteExiste = await _context.Docentes
                .AnyAsync(d => d.Id == dto.DocenteId);

            if (!docenteExiste)
                return NotFound("El docente no existe");

            var duplicado = await _context.Cursos
                .AnyAsync(c =>
                    c.MateriaId == dto.MateriaId &&
                    c.DocenteId == dto.DocenteId &&
                    c.Id != id);

            if (duplicado)
                return BadRequest("Ya existe otro curso con esa materia y docente");

            curso.MateriaId = dto.MateriaId;
            curso.DocenteId = dto.DocenteId;
            curso.CupoMaximo = dto.CupoMaximo;

            await _context.SaveChangesAsync();

            return Ok(curso);
        }

        // ✅ DELETE: api/cursos/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> EliminarCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
                return NotFound("Curso no encontrado");

            var tieneAsignaciones = await _context.Asignaciones
                .AnyAsync(a => a.CursoId == id);

            if (tieneAsignaciones)
                return BadRequest("No se puede eliminar el curso porque tiene asignaciones activas");

            var tieneMatriculas = await _context.Matriculas
                .AnyAsync(m => m.CursoId == id);

            if (tieneMatriculas)
                return BadRequest("No se puede eliminar el curso porque tiene estudiantes matriculados");

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Curso eliminado correctamente" });
        }

    }
}
