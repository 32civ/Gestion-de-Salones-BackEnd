using GestionSalones.Data;
using GestionSalones.Helpers;
using GestionSalones.Models;
using GestionSalones.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionSalones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class DocentesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocentesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/docentes
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> GetDocentes()
        {
            var docentes = await _context.Docentes
                .AsNoTracking()
                .Select(d => new DocenteDTO
                {
                    Id = d.Id,
                    Nombre = d.Usuario.Nombre,
                    Email = d.Usuario.Email,
                    Activo = d.Usuario.Activo
                })
                .ToListAsync<DocenteDTO>();

            return Ok(docentes);
        }

        // ✅ GET: api/docentes/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente)]
        public async Task<IActionResult> GetDocente(int id)
        {
            var docente = await _context.Docentes
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (docente == null)
                return NotFound("Docente no encontrado");

            return Ok(new
            {
                docente.Id,
                docente.Usuario.Nombre,
                docente.Usuario.Email,
                docente.Usuario.Activo
            });
        }

        // ✅ GET: api/docentes/5/cursos
        [HttpGet("{id}/cursos")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente)]
        public async Task<IActionResult> GetCursosDocente(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);

            if (docente == null)
                return NotFound("Docente no encontrado");

            var cursos = await _context.Cursos
                .Include(c => c.Materia)
                .Where(c => c.DocenteId == id)
                .Select(c => new
                {
                    c.Id,
                    Materia = c.Materia.Nombre,
                    c.CupoMaximo,
                    SalonAsignado = _context.Asignaciones
                        .Where(a => a.CursoId == c.Id)
                        .Select(a => a.Salon.Nombre)
                        .FirstOrDefault() ?? "Sin asignar"
                })
                .ToListAsync<object>();

            return Ok(cursos);
        }

        // ✅ POST: api/docentes
        [HttpPost]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> CrearDocente(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // Verificar que el usuario no sea ya docente
            var yaEsDocente = await _context.Docentes
                .AnyAsync(d => d.UsuarioId == usuarioId);

            if (yaEsDocente)
                return BadRequest("Este usuario ya está registrado como docente");

            // Verificar que el usuario tenga el rol de docente
            var tieneRolDocente = await _context.UsuarioRoles
                .Include(ur => ur.Rol)
                .AnyAsync(ur => ur.UsuarioId == usuarioId && ur.Rol.Nombre == Roles.Docente);

            if (!tieneRolDocente)
                return BadRequest("El usuario no tiene el rol de Docente asignado");

            var docente = new Docente { UsuarioId = usuarioId };
            _context.Docentes.Add(docente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocente), new { id = docente.Id }, new
            {
                docente.Id,
                usuario.Nombre,
                usuario.Email
            });
        }

        // ✅ DELETE: api/docentes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrativo)]
        public async Task<IActionResult> EliminarDocente(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);

            if (docente == null)
                return NotFound("Docente no encontrado");

            // Verificar si tiene cursos asignados
            var tieneCursos = await _context.Cursos
                .AnyAsync(c => c.DocenteId == id);

            if (tieneCursos)
                return BadRequest("No se puede eliminar el docente porque tiene cursos asignados");

            _context.Docentes.Remove(docente);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Docente eliminado correctamente" });
        }

    }
}
