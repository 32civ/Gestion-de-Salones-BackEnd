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
    public class EstudiantesController : ControllerBase
    {

        private readonly AppDbContext _context;

        public EstudiantesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/estudiantes
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetEstudiantes()
        {
            var estudiantes = await _context.Estudiantes
                .Include(e => e.Usuario)
                .Select(e => new
                {
                    e.Id,
                    e.UsuarioId,
                    e.Usuario.Nombre,
                    e.Usuario.Email,
                    e.Usuario.Activo,
                    TotalMaterias = _context.Matriculas
                        .Count(m => m.EstudianteId == e.Id)
                })
                .ToListAsync<object>();

            return Ok(estudiantes);
        }

        // ✅ GET: api/estudiantes/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estudiante == null)
                return NotFound("Estudiante no encontrado");

            return Ok(new
            {
                estudiante.Id,
                estudiante.Usuario.Nombre,
                estudiante.Usuario.Email,
                estudiante.Usuario.Activo
            });
        }

        // ✅ GET: api/estudiantes/mi-perfil
        [HttpGet("mi-perfil")]
        [Authorize(Roles = Roles.Estudiante)]
        public async Task<IActionResult> GetMiPerfil()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var estudiante = await _context.Estudiantes
                .Include(e => e.Carrera) // ✅ incluir carrera
                .FirstOrDefaultAsync(e => e.UsuarioId == usuario.Id);

            if (estudiante == null)
                return NotFound("No estás registrado como estudiante");

            var totalMaterias = await _context.Matriculas
                .CountAsync(m => m.EstudianteId == estudiante.Id);

            return Ok(new
            {
                estudiante.Id,
                usuario.Nombre,
                usuario.Email,
                usuario.Activo,
                Carrera = estudiante.Carrera?.Nombre ?? "Sin carrera asignada",
                CarreraId = estudiante.CarreraId,
                TotalMateriasMatriculadas = totalMaterias
            });
        }

        // ✅ POST: api/estudiantes
        [HttpPost]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> CrearEstudiante(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // Verificar que no sea ya estudiante
            var yaEsEstudiante = await _context.Estudiantes
                .AnyAsync(e => e.UsuarioId == usuarioId);

            if (yaEsEstudiante)
                return BadRequest("Este usuario ya está registrado como estudiante");

            // Verificar que tenga el rol de estudiante
            var tieneRolEstudiante = await _context.UsuarioRoles
                .Include(ur => ur.Rol)
                .AnyAsync(ur => ur.UsuarioId == usuarioId
                             && ur.Rol.Nombre == Roles.Estudiante);

            if (!tieneRolEstudiante)
                return BadRequest("El usuario no tiene el rol de Estudiante asignado");

            var estudiante = new Estudiante { UsuarioId = usuarioId };
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.Id }, new
            {
                estudiante.Id,
                usuario.Nombre,
                usuario.Email
            });
        }

        // ✅ DELETE: api/estudiantes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> EliminarEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound("Estudiante no encontrado");

            // Verificar si tiene matrículas activas
            var tieneMatriculas = await _context.Matriculas
                .AnyAsync(m => m.EstudianteId == id);

            if (tieneMatriculas)
                return BadRequest("No se puede eliminar el estudiante porque tiene matrículas activas");

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Estudiante eliminado correctamente" });
        }

        // 🔵 GET: api/estudiantes/calificaciones
        // Base lista para implementar después
        [HttpGet("calificaciones")]
        [Authorize(Roles = Roles.Estudiante)]
        public IActionResult GetCalificaciones()
        {
            return Ok(new { message = "Sistema de calificaciones próximamente" });
        }

        // ✅ PUT: api/estudiantes/5/carrera
        [HttpPut("{id}/carrera")]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> AsignarCarrera(int id, [FromBody] int carreraId)
        {
            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estudiante == null)
                return NotFound("Estudiante no encontrado");

            var carreraExiste = await _context.Carreras
                .AnyAsync(c => c.Id == carreraId);

            if (!carreraExiste)
                return NotFound("Carrera no encontrada");

            estudiante.CarreraId = carreraId;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Carrera asignada correctamente" });
        }

    }
}
