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

    public class SalonesController: ControllerBase
    {
        private readonly AppDbContext _context;

        public SalonesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/salones
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetSalones()
        {
            var salones = await _context.Salones
                .Select(s => new
                {
                    s.Id,
                    s.Nombre,
                    s.Capacidad,
                    Recursos = s.SalonRecursos.Select(sr => sr.Recurso.Nombre).ToList()
                })
                .ToListAsync<object>();

            return Ok(salones);
        }

        // ✅ GET: api/salones/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetSalon(int id)
        {
            var salon = await _context.Salones
                .Include(s => s.SalonRecursos)
                    .ThenInclude(sr => sr.Recurso)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salon == null)
                return NotFound("Salón no encontrado");

            return Ok(new
            {
                salon.Id,
                salon.Nombre,
                salon.Capacidad,
                Recursos = salon.SalonRecursos.Select(sr => sr.Recurso.Nombre).ToList()
            });
        }

        // ✅ GET: api/salones/disponibles
        [HttpGet("disponibles")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetSalonesDisponibles(int horarioId, int cupoMinimo = 0)
        {
            // Obtener IDs de salones ocupados en ese horario
            var salonesOcupados = await _context.Asignaciones
                .Where(a => a.HorarioId == horarioId)
                .Select(a => a.SalonId)
                .ToListAsync();

            // Filtrar salones que no estén ocupados y cumplan el cupo
            var salonesDisponibles = await _context.Salones
                .Where(s => !salonesOcupados.Contains(s.Id) && s.Capacidad >= cupoMinimo)
                .Select(s => new
                {
                    s.Id,
                    s.Nombre,
                    s.Capacidad,
                    Recursos = s.SalonRecursos.Select(sr => sr.Recurso.Nombre).ToList()
                })
                .OrderBy(s => s.Capacidad) // Ordena del más pequeño al más grande
                .ToListAsync<object>();

            if (!salonesDisponibles.Any())
                return NotFound("No hay salones disponibles para ese horario y cupo");

            return Ok(salonesDisponibles);
        }

        // ✅ POST: api/salones
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CrearSalon(Salon salon)
        {
            if (string.IsNullOrWhiteSpace(salon.Nombre))
                return BadRequest("El nombre es obligatorio");

            if (salon.Capacidad <= 0)
                return BadRequest("La capacidad debe ser mayor a 0");

            var existe = await _context.Salones
                .AnyAsync(s => s.Nombre.ToLower().Trim() == salon.Nombre.ToLower().Trim());

            if (existe)
                return BadRequest("Ya existe un salón con ese nombre");

            _context.Salones.Add(salon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalon), new { id = salon.Id }, new
            {
                salon.Id,
                salon.Nombre,
                salon.Capacidad
            });
        }

        // ✅ PUT: api/salones/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> EditarSalon(int id, Salon salonEditado)
        {
            var salon = await _context.Salones.FindAsync(id);

            if (salon == null)
                return NotFound("Salón no encontrado");

            if (string.IsNullOrWhiteSpace(salonEditado.Nombre))
                return BadRequest("El nombre es obligatorio");

            if (salonEditado.Capacidad <= 0)
                return BadRequest("La capacidad debe ser mayor a 0");

            // Verificar nombre duplicado en otro salón
            var nombreDuplicado = await _context.Salones
                .AnyAsync(s => s.Nombre.ToLower().Trim() == salonEditado.Nombre.ToLower().Trim() && s.Id != id);

            if (nombreDuplicado)
                return BadRequest("Ya existe otro salón con ese nombre");

            salon.Nombre = salonEditado.Nombre;
            salon.Capacidad = salonEditado.Capacidad;
            await _context.SaveChangesAsync();

            return Ok(salon);
        }

        // ✅ DELETE: api/salones/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> EliminarSalon(int id)
        {
            var salon = await _context.Salones
                .Include(s => s.SalonRecursos)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salon == null)
                return NotFound("Salón no encontrado");

            // Verificar si tiene asignaciones activas
            var tieneAsignaciones = await _context.Asignaciones
                .AnyAsync(a => a.SalonId == id);

            if (tieneAsignaciones)
                return BadRequest("No se puede eliminar el salón porque tiene asignaciones activas");

            _context.Salones.Remove(salon);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Salón eliminado correctamente" });
        }

        // ✅ POST: api/salones/5/recursos
        [HttpPost("{id}/recursos")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AgregarRecurso(int id, int recursoId)
        {
            var salon = await _context.Salones.FindAsync(id);
            if (salon == null)
                return NotFound("Salón no encontrado");

            var recurso = await _context.Recursos.FindAsync(recursoId);
            if (recurso == null)
                return NotFound("Recurso no encontrado");

            // Verificar si ya tiene ese recurso
            var yaExiste = await _context.SalonRecursos
                .AnyAsync(sr => sr.SalonId == id && sr.RecursoId == recursoId);

            if (yaExiste)
                return BadRequest("El salón ya tiene ese recurso");

            _context.SalonRecursos.Add(new SalonRecurso
            {
                SalonId = id,
                RecursoId = recursoId
            });

            await _context.SaveChangesAsync();

            return Ok(new { message = "Recurso agregado al salón correctamente" });
        }

        // ✅ DELETE: api/salones/5/recursos/2
        [HttpDelete("{id}/recursos/{recursoId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> QuitarRecurso(int id, int recursoId)
        {
            var salonRecurso = await _context.SalonRecursos
                .FirstOrDefaultAsync(sr => sr.SalonId == id && sr.RecursoId == recursoId);

            if (salonRecurso == null)
                return NotFound("El salón no tiene ese recurso");

            _context.SalonRecursos.Remove(salonRecurso);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Recurso eliminado del salón correctamente" });
        }


    }
}
