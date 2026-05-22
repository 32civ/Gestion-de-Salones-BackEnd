using GestionSalones.Data;
using GestionSalones.DTOs;
using GestionSalones.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalones.Helpers;

namespace GestionSalones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador, Administrativo")]
    public class RolesController : ControllerBase
    {

        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                .Select(r => new
                {
                    r.Id,
                    r.Nombre
                })
                .ToListAsync();

            return Ok(roles);
        }

        // ✅ GET: api/roles/usuarios/5
        // Obtiene los roles de un usuario
        [HttpGet("usuarios/{usuarioId}")]
        public async Task<IActionResult> GetRolesUsuario(int usuarioId)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            var roles = await _context.UsuarioRoles
                .Where(ur => ur.UsuarioId == usuarioId)
                .Include(ur => ur.Rol)
                .Select(ur => new
                {
                    ur.RolId,
                    ur.Rol.Nombre
                })
                .ToListAsync();

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Email,
                Roles = roles
            });
        }

        // ✅ POST: api/roles/asignar
        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarRol(AsignarRolDTO dto)
        {
            // Verificar usuario
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == dto.UsuarioId);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // Verificar rol
            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == dto.RolId);

            if (rol == null)
                return NotFound("Rol no encontrado");

            // Verificar si ya tiene el rol
            var yaExiste = await _context.UsuarioRoles
                .AnyAsync(ur =>
                    ur.UsuarioId == dto.UsuarioId &&
                    ur.RolId == dto.RolId);

            if (yaExiste)
                return BadRequest("El usuario ya tiene ese rol asignado");

            var usuarioRol = new UsuarioRol
            {
                UsuarioId = dto.UsuarioId,
                RolId = dto.RolId
            };

            _context.UsuarioRoles.Add(usuarioRol);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Rol asignado correctamente",
                Usuario = usuario.Nombre,
                Rol = rol.Nombre
            });
        }

        // ✅ DELETE: api/roles/quitar
        [HttpDelete("quitar")]
        public async Task<IActionResult> QuitarRol(AsignarRolDTO dto)
        {
            var usuarioRol = await _context.UsuarioRoles
                .Include(ur => ur.Usuario)
                .Include(ur => ur.Rol)
                .FirstOrDefaultAsync(ur =>
                    ur.UsuarioId == dto.UsuarioId &&
                    ur.RolId == dto.RolId);

            if (usuarioRol == null)
                return NotFound("El usuario no tiene ese rol");

            _context.UsuarioRoles.Remove(usuarioRol);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Rol removido correctamente",
                Usuario = usuarioRol.Usuario.Nombre,
                Rol = usuarioRol.Rol.Nombre
            });
        }

        // ✅ POST: api/roles
        // Crear un nuevo rol
        [HttpPost]
        [Authorize(Roles = "Administrador Académico")]
        public async Task<IActionResult> CrearRol([FromBody] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre del rol es obligatorio");

            var existe = await _context.Roles
                .AnyAsync(r => r.Nombre.ToLower().Trim() == nombre.ToLower().Trim());

            if (existe)
                return BadRequest("Ya existe un rol con ese nombre");

            var rol = new Rol
            {
                Nombre = nombre.Trim()
            };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Rol creado correctamente",
                rol.Id,
                rol.Nombre
            });
        }

        // ✅ DELETE: api/roles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador Académico")]
        public async Task<IActionResult> EliminarRol(int id)
        {
            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rol == null)
                return NotFound("Rol no encontrado");

            // Verificar si está asignado a usuarios
            var tieneUsuarios = await _context.UsuarioRoles
                .AnyAsync(ur => ur.RolId == id);

            if (tieneUsuarios)
                return BadRequest("No se puede eliminar el rol porque está asignado a usuarios");

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Rol eliminado correctamente"
            });
        }

    }
}
