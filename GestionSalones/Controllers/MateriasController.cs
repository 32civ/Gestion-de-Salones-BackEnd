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

    public class MateriasController : ControllerBase
    {

        private readonly AppDbContext _context;

        public MateriasController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/materias
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetMaterias()
        {
            var materias = await _context.Materias
                .AsNoTracking()
                .Select(m => new
                {
                    m.Id,
                    m.Nombre,
                    Carrera = m.Carrera.Nombre
                })
                .ToListAsync();

            return Ok(materias);
        }

        // ✅ GET: api/materias/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetMateria(int id)
        {
            var materia = await _context.Materias
                .AsNoTracking()
                .Select(m => new MateriaDTO
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    CarreraId = m.CarreraId,
                    CarreraNombre = m.Carrera.Nombre
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (materia == null)
                return NotFound("Materia no encontrada");

            return Ok(new
            {
                materia.Id,
                materia.Nombre,
                Carrera = materia.CarreraNombre,
                materia.CarreraId
            });
        }

        // ✅ GET: api/materias/carrera/3
        [HttpGet("carrera/{carreraId}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetMateriasPorCarrera(int carreraId)
        {
            var carrera = await _context.Carreras.FindAsync(carreraId);

            if (carrera == null)
                return NotFound("Carrera no encontrada");

            var materias = await _context.Materias
                .Where(m => m.CarreraId == carreraId)
                .Select(m => new
                {
                    m.Id,
                    m.Nombre
                })
                .ToListAsync<object>();

            return Ok(materias);
        }

        // ✅ POST: api/materias
        [HttpPost]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> CrearMateria(Materia materia)
        {
            if (string.IsNullOrWhiteSpace(materia.Nombre))
                return BadRequest("El nombre es obligatorio");

            // Verificar que la carrera exista
            var carreraExiste = await _context.Carreras
                .AsNoTracking()
                .AnyAsync(c => c.Id == materia.CarreraId);

            if (!carreraExiste)
                return NotFound("La carrera especificada no existe");

            // Verificar que no exista la misma materia en la misma carrera
            var existe = await _context.Materias
                .AnyAsync(m => m.Nombre.ToLower().Trim() == materia.Nombre.ToLower().Trim()
                            && m.CarreraId == materia.CarreraId);

            if (existe)
                return BadRequest("Ya existe una materia con ese nombre en esta carrera");

            _context.Materias.Add(materia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMateria), new { id = materia.Id }, new
            {
                materia.Id,
                materia.Nombre,
                materia.CarreraId
            });
        }

        // ✅ PUT: api/materias/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> EditarMateria(int id, Materia materiaEditada)
        {
            var materia = await _context.Materias.FindAsync(id);

            if (materia == null)
                return NotFound("Materia no encontrada");

            if (string.IsNullOrWhiteSpace(materiaEditada.Nombre))
                return BadRequest("El nombre es obligatorio");

            var carreraExiste = await _context.Carreras
                .AnyAsync(c => c.Id == materiaEditada.CarreraId);

            if (!carreraExiste)
                return NotFound("La carrera especificada no existe");

            // Verificar duplicado en la misma carrera
            var duplicado = await _context.Materias
                .AnyAsync(m => m.Nombre.ToLower().Trim() == materiaEditada.Nombre.ToLower().Trim()
                            && m.CarreraId == materiaEditada.CarreraId
                            && m.Id != id);

            if (duplicado)
                return BadRequest("Ya existe otra materia con ese nombre en esta carrera");

            materia.Nombre = materiaEditada.Nombre;
            materia.CarreraId = materiaEditada.CarreraId;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                materia.Id,
                materia.Nombre,
                materia.CarreraId
            });
        }

        // ✅ DELETE: api/materias/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> EliminarMateria(int id)
        {
            var materia = await _context.Materias.FindAsync(id);

            if (materia == null)
                return NotFound("Materia no encontrada");

            // Verificar si tiene cursos asociados
            var tieneCursos = await _context.Cursos
                .AnyAsync(c => c.MateriaId == id);

            if (tieneCursos)
                return BadRequest("No se puede eliminar la materia porque tiene cursos asociados");

            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Materia eliminada correctamente" });
        }
    }
}
