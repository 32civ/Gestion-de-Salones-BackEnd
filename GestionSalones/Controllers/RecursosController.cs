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

    public class RecursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecursosController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/recursos
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetRecursos()
        {
            var recursos = await _context.Recursos
                .Select(r => new RecursoDTO
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    TotalSalones = r.SalonRecursos.Count
                })
                .ToListAsync<RecursoDTO>();


            return Ok(recursos);
        }

        // ✅ GET: api/recursos/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetRecurso(int id)
        {
            var recurso = await _context.Recursos
                .Include(r => r.SalonRecursos)
                    .ThenInclude(sr => sr.Salon)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recurso == null)
                return NotFound("Recurso no encontrado");

            return Ok(new
            {
                recurso.Id,
                recurso.Nombre,
                Salones = recurso.SalonRecursos.Select(sr => new
                {
                    sr.Salon.Id,
                    sr.Salon.Nombre,
                    sr.Salon.Capacidad
                }).ToList()
            });
        }

        // ✅ POST: api/recursos
        [HttpPost]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> CrearRecurso(CreateRecursoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio");

            var existe = await _context.Recursos
                .AnyAsync(r =>
                    r.Nombre.ToLower().Trim() ==
                    dto.Nombre.ToLower().Trim());

            if (existe)
                return BadRequest("Ya existe un recurso con ese nombre");

            var recurso = new Recursos
            {
                Nombre = dto.Nombre.Trim()
            };

            _context.Recursos.Add(recurso);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRecurso),
                new { id = recurso.Id },
                new
                {
                    recurso.Id,
                    recurso.Nombre
                });
        }

        // ✅ PUT: api/recursos/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> EditarRecurso(int id, Recursos recursoEditado)
        {
            var recurso = await _context.Recursos.FindAsync(id);

            if (recurso == null)
                return NotFound("Recurso no encontrado");

            if (string.IsNullOrWhiteSpace(recursoEditado.Nombre))
                return BadRequest("El nombre es obligatorio");

            var nombreDuplicado = await _context.Recursos
                .AnyAsync(r => r.Nombre.ToLower().Trim() == recursoEditado.Nombre.ToLower().Trim() && r.Id != id);

            if (nombreDuplicado)
                return BadRequest("Ya existe otro recurso con ese nombre");

            recurso.Nombre = recursoEditado.Nombre;
            await _context.SaveChangesAsync();

            return Ok(recurso);
        }

        // ✅ DELETE: api/recursos/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> EliminarRecurso(int id)
        {
            var recurso = await _context.Recursos
                .Include(r => r.SalonRecursos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recurso == null)
                return NotFound("Recurso no encontrado");

            // Evitar eliminar si está asignado a algún salón
            if (recurso.SalonRecursos.Any())
                return BadRequest($"No se puede eliminar el recurso porque está asignado a {recurso.SalonRecursos.Count} salón(es)");

            _context.Recursos.Remove(recurso);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Recurso eliminado correctamente" });
        }
    }
}
