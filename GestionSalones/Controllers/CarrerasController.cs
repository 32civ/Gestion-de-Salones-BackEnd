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

    public class CarrerasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarrerasController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/carreras
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetCarreras()
        {
            var carreras = await _context.Carreras
                .Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    TotalMaterias = c.Materias.Count
                })
                .ToListAsync<object>();

            return Ok(carreras);
        }

        // ✅ GET: api/carreras/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetCarrera(int id)
        {
            var carrera = await _context.Carreras
                .Include(c => c.Materias)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrera == null)
                return NotFound("Carrera no encontrada");

            return Ok(carrera);
        }

        // ✅ POST: api/carreras
        [HttpPost]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> CrearCarrera(Carrera carrera)
        {
            //Validar que ingresen datos
            if (string.IsNullOrWhiteSpace(carrera.Nombre))
                return BadRequest("El nombre es obligatorio");

            // Verificar si ya existe una carrera con ese nombre
            var existe = await _context.Carreras
                .AnyAsync(c => c.Nombre.ToLower().Trim() == carrera.Nombre.ToLower().Trim());

            if (existe)
                return BadRequest("Ya existe una carrera con ese nombre");

            _context.Carreras.Add(carrera);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarrera), new { id = carrera.Id }, carrera);
        }

        // ✅ PUT: api/carreras/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> EditarCarrera(int id, Carrera carreraEditada)
        {
            var carrera = await _context.Carreras.FindAsync(id);

            if (carrera == null)
                return NotFound("Carrera no encontrada");

            // Verificar que no exista otra carrera con el mismo nombre
            var nombreDuplicado = await _context.Carreras
                .AnyAsync(c => c.Nombre.ToLower() == carreraEditada.Nombre.ToLower() && c.Id != id);

            if (nombreDuplicado)
                return BadRequest("Ya existe otra carrera con ese nombre");

            carrera.Nombre = carreraEditada.Nombre;
            await _context.SaveChangesAsync();

            return Ok(carrera);
        }

        // ✅ DELETE: api/carreras/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrativo + "," + Roles.Admin)]
        public async Task<IActionResult> EliminarCarrera(int id)
        {
            var carrera = await _context.Carreras
                .Include(c => c.Materias)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrera == null)
                return NotFound("Carrera no encontrada");

            // Evitar eliminar si tiene materias asociadas
            if (carrera.Materias.Any())
                return BadRequest($"No se puede eliminar la carrera porque tiene {carrera.Materias.Count} materia(s) asociada(s)");

            _context.Carreras.Remove(carrera);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Carrera eliminada correctamente" });
        }
    }
}

