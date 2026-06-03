using GestionSalones.Data;
using GestionSalones.Helpers;
using GestionSalones.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalones.DTOs;

namespace GestionSalones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // ← aquí
    public class SemestresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SemestresController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/semestres
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetSemestres()
        {
            var semestres = await _context.Semestres
                .OrderByDescending(s => s.FechaInicio)
                .Select(s => new
                {
                    s.Id,
                    s.Nombre,
                    FechaInicio = s.FechaInicio.ToString("yyyy-MM-dd"),
                    FechaFin = s.FechaFin.ToString("yyyy-MM-dd"),
                    EsActivo = DateTime.Now >= s.FechaInicio && DateTime.Now <= s.FechaFin,
                    TotalCursos = s.Cursos.Count
                })
                .ToListAsync<object>();

            return Ok(semestres);
        }

        // ✅ GET: api/semestres/activo
        [HttpGet("activo")]
        [Authorize]
        public async Task<IActionResult> GetSemestreActivo()
        {
            var ahora = DateTime.Now;

            var semestre = await _context.Semestres
                .Where(s => s.FechaInicio <= ahora && s.FechaFin >= ahora)
                .Select(s => new
                {
                    s.Id,
                    s.Nombre,
                    FechaInicio = s.FechaInicio.ToString("yyyy-MM-dd"),
                    FechaFin = s.FechaFin.ToString("yyyy-MM-dd"),
                })
                .FirstOrDefaultAsync();

            if (semestre == null)
                return NotFound("No hay un semestre activo en este momento");

            return Ok(semestre);
        }

        // ✅ GET: api/semestres/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetSemestre(int id)
        {
            var semestre = await _context.Semestres.FindAsync(id);

            if (semestre == null)
                return NotFound("Semestre no encontrado");

            return Ok(new
            {
                semestre.Id,
                semestre.Nombre,
                FechaInicio = semestre.FechaInicio.ToString("yyyy-MM-dd"),
                FechaFin = semestre.FechaFin.ToString("yyyy-MM-dd"),
                EsActivo = DateTime.Now >= semestre.FechaInicio && DateTime.Now <= semestre.FechaFin,
            });
        }

        // ✅ POST: api/semestres
        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> CrearSemestre([FromBody] CrearSemestreDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio");

            if (dto.FechaFin <= dto.FechaInicio)
                return BadRequest("La fecha de fin debe ser mayor a la de inicio");

            // Verificar solapamiento con otro semestre
            var solapamiento = await _context.Semestres
                .AnyAsync(s =>
                    dto.FechaInicio < s.FechaFin &&
                    dto.FechaFin > s.FechaInicio);

            if (solapamiento)
                return BadRequest("Las fechas se solapan con otro semestre existente");

            // Verificar nombre duplicado
            var nombreDuplicado = await _context.Semestres
                .AnyAsync(s => s.Nombre.ToLower() == dto.Nombre.ToLower().Trim());

            if (nombreDuplicado)
                return BadRequest("Ya existe un semestre con ese nombre");

            var semestre = new Semestre
            {
                Nombre = dto.Nombre.Trim(),
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin
            };

            _context.Semestres.Add(semestre);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSemestre), new { id = semestre.Id }, new
            {
                semestre.Id,
                semestre.Nombre,
                FechaInicio = semestre.FechaInicio.ToString("yyyy-MM-dd"),
                FechaFin = semestre.FechaFin.ToString("yyyy-MM-dd"),
            });
        }

        // ✅ PUT: api/semestres/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> EditarSemestre(int id, [FromBody] CrearSemestreDTO dto)
        {
            var semestre = await _context.Semestres.FindAsync(id);

            if (semestre == null)
                return NotFound("Semestre no encontrado");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio");

            if (dto.FechaFin <= dto.FechaInicio)
                return BadRequest("La fecha de fin debe ser mayor a la de inicio");

            // Verificar solapamiento excluyendo el semestre actual
            var solapamiento = await _context.Semestres
                .AnyAsync(s =>
                    s.Id != id &&
                    dto.FechaInicio < s.FechaFin &&
                    dto.FechaFin > s.FechaInicio);

            if (solapamiento)
                return BadRequest("Las fechas se solapan con otro semestre existente");

            var nombreDuplicado = await _context.Semestres
                .AnyAsync(s => s.Nombre.ToLower() == dto.Nombre.ToLower().Trim() && s.Id != id);

            if (nombreDuplicado)
                return BadRequest("Ya existe otro semestre con ese nombre");

            semestre.Nombre = dto.Nombre.Trim();
            semestre.FechaInicio = dto.FechaInicio;
            semestre.FechaFin = dto.FechaFin;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                semestre.Id,
                semestre.Nombre,
                FechaInicio = semestre.FechaInicio.ToString("yyyy-MM-dd"),
                FechaFin = semestre.FechaFin.ToString("yyyy-MM-dd"),
            });
        }

        // ✅ DELETE: api/semestres/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> EliminarSemestre(int id)
        {
            var semestre = await _context.Semestres
                .Include(s => s.Cursos)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (semestre == null)
                return NotFound("Semestre no encontrado");

            if (semestre.Cursos.Any())
                return BadRequest($"No se puede eliminar el semestre porque tiene {semestre.Cursos.Count} curso(s) asociado(s)");

            // Verificar si es el activo
            if (DateTime.Now >= semestre.FechaInicio && DateTime.Now <= semestre.FechaFin)
                return BadRequest("No se puede eliminar el semestre activo");

            _context.Semestres.Remove(semestre);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Semestre eliminado correctamente" });
        }
    }
}
