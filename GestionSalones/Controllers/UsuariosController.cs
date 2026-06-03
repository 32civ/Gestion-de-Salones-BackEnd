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
    [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // ✅ GET: api/usuarios
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    u.Email,
                    u.Activo,
                    Roles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList(),
                    Carrera = _context.Estudiantes
                    .Where(e => e.UsuarioId == u.Id)
                    .Select(e => e.Carrera.Nombre)
                    .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // =========================================================
        // ✅ GET: api/usuarios/5
        // =========================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Email,
                usuario.Activo,
                Roles = usuario.UsuarioRoles
                    .Select(ur => ur.Rol.Nombre)
                    .ToList()
            });
        }

        // =========================================================
        // ✅ POST: api/usuarios
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> CrearUsuario(CrearUsuarioDTO dto)
        {
            // =====================================================
            // ✅ Validaciones
            // =====================================================

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("El email es obligatorio");

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("La contraseña es obligatoria");

            if (dto.Password.Length < 6)
                return BadRequest("La contraseña debe tener mínimo 6 caracteres");

            // =====================================================
            // ✅ Normalizar email
            // =====================================================

            var email = dto.Email.Trim().ToLower();

            // =====================================================
            // ✅ Verificar duplicados
            // =====================================================

            var existe = await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == email);

            if (existe)
                return BadRequest("Ya existe un usuario con ese email");

            // =====================================================
            // ✅ Crear usuario
            // =====================================================

            var usuario = new Usuarios
            {
                Nombre = dto.Nombre.Trim(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // =====================================================
            // ✅ Asignar rol automáticamente
            // =====================================================

            if (!string.IsNullOrWhiteSpace(dto.Rol))
            {
                var rol = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Nombre == dto.Rol);

                if (rol == null)
                    return BadRequest("El rol especificado no existe");

                _context.UsuarioRoles.Add(new UsuarioRol
                {
                    UsuarioId = usuario.Id,
                    RolId = rol.Id
                });

                // ================================================
                // ✅ Crear entidades especiales
                // ================================================

                if (rol.Nombre == Roles.Docente)
                {
                    _context.Docentes.Add(new Docente
                    {
                        UsuarioId = usuario.Id
                    });
                }

                if (rol.Nombre == Roles.Estudiante)
                {
                    _context.Estudiantes.Add(new Estudiante
                    {
                        UsuarioId = usuario.Id
                    });
                }

                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                message = "Usuario creado correctamente",
                usuario = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Email,
                    usuario.Activo,
                    dto.Rol
                }
            });
        }

        // =========================================================
        // ✅ PUT: api/usuarios/5
        // =========================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarUsuario(int id, EditarUsuarioDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // =====================================================
            // ✅ Validaciones
            // =====================================================

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("El email es obligatorio");

            var email = dto.Email.Trim().ToLower();

            // =====================================================
            // ✅ Verificar email duplicado
            // =====================================================

            var emailDuplicado = await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == email && u.Id != id);

            if (emailDuplicado)
                return BadRequest("Ya existe otro usuario con ese email");

            // =====================================================
            // ✅ Actualizar datos
            // =====================================================

            usuario.Nombre = dto.Nombre.Trim();
            usuario.Email = email;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario actualizado correctamente",
                usuario.Id,
                usuario.Nombre,
                usuario.Email
            });
        }

        // =========================================================
        // ✅ PUT: api/usuarios/5/password
        // =========================================================
        [HttpPut("{id}/password")]
        public async Task<IActionResult> CambiarPassword(int id, CambiarPasswordDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            if (string.IsNullOrWhiteSpace(dto.NuevaPassword))
                return BadRequest("La nueva contraseña es obligatoria");

            if (dto.NuevaPassword.Length < 6)
                return BadRequest("La contraseña debe tener mínimo 6 caracteres");

            // =====================================================
            // ✅ Actualizar contraseña
            // =====================================================

            usuario.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Contraseña actualizada correctamente"
            });
        }

        // =========================================================
        // ✅ PUT: api/usuarios/5/activar
        // =========================================================
        [HttpPut("{id}/activar")]
        public async Task<IActionResult> ActivarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            if (usuario.Activo)
                return BadRequest("El usuario ya está activo");

            usuario.Activo = true;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario activado correctamente"
            });
        }

        // =========================================================
        // ✅ PUT: api/usuarios/5/desactivar
        // =========================================================
        [HttpPut("{id}/desactivar")]
        public async Task<IActionResult> DesactivarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            if (!usuario.Activo)
                return BadRequest("El usuario ya está desactivado");

            usuario.Activo = false;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario desactivado correctamente"
            });
        }

        // =========================================================
        // ✅ DELETE: api/usuarios/5
        // =========================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // ✅ Evitar eliminar admins
            var esAdmin = usuario.UsuarioRoles
                .Any(ur => ur.Rol.Nombre == Roles.Admin);

            if (esAdmin)
                return BadRequest("No se puede eliminar un administrador");

            // ✅ Validaciones especiales para docentes
            var docente = await _context.Docentes
                .FirstOrDefaultAsync(d => d.UsuarioId == id);

            if (docente != null)
            {
                // 1️⃣ Debe estar desactivado para poder eliminarlo
                if (usuario.Activo)
                    return BadRequest("Debes desactivar al docente antes de eliminarlo");

                // 2️⃣ No puede tener cursos con asignaciones activas
                var tieneAsignacionesActivas = await _context.Asignaciones
                    .AnyAsync(a =>
                        a.Curso.DocenteId == docente.Id &&
                        (a.Estado == "Pendiente" || a.Estado == "Aprobado")
                    );

                if (tieneAsignacionesActivas)
                    return BadRequest("El docente tiene asignaciones activas. Cancélalas antes de eliminar al docente");

                // 3️⃣ Eliminar registros relacionados en orden correcto
                // Aprobaciones → Asignaciones → Cursos → Docente

                var cursoIds = await _context.Cursos
                    .Where(c => c.DocenteId == docente.Id)
                    .Select(c => c.Id)
                    .ToListAsync();

                if (cursoIds.Any())
                {
                    var asignacionIds = await _context.Asignaciones
                        .Where(a => cursoIds.Contains(a.CursoId))
                        .Select(a => a.Id)
                        .ToListAsync();

                    if (asignacionIds.Any())
                    {
                        var aprobaciones = await _context.AprobacionesDocente
                            .Where(ap => asignacionIds.Contains(ap.AsignacionId))
                            .ToListAsync();
                        _context.AprobacionesDocente.RemoveRange(aprobaciones);

                        var asignaciones = await _context.Asignaciones
                            .Where(a => asignacionIds.Contains(a.Id))
                            .ToListAsync();
                        _context.Asignaciones.RemoveRange(asignaciones);
                    }

                    // Verificar que los cursos no tengan matrículas
                    var tieneMatriculas = await _context.Matriculas
                        .AnyAsync(m => cursoIds.Contains(m.CursoId));

                    if (tieneMatriculas)
                        return BadRequest("El docente tiene cursos con estudiantes matriculados. Cancela las matrículas antes de eliminar al docente");

                    var cursos = await _context.Cursos
                        .Where(c => c.DocenteId == docente.Id)
                        .ToListAsync();
                    _context.Cursos.RemoveRange(cursos);
                }

                _context.Docentes.Remove(docente);
            }

            // ✅ Eliminar estudiante si aplica
            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.UsuarioId == id);

            if (estudiante != null)
            {
                var tieneMatriculas = await _context.Matriculas
                    .AnyAsync(m => m.EstudianteId == estudiante.Id);

                if (tieneMatriculas)
                    return BadRequest("El estudiante tiene matrículas activas. Cancélalas antes de eliminarlo");

                _context.Estudiantes.Remove(estudiante);
            }

            // ✅ Eliminar roles y usuario
            var relaciones = await _context.UsuarioRoles
                .Where(ur => ur.UsuarioId == id)
                .ToListAsync();
            _context.UsuarioRoles.RemoveRange(relaciones);

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario eliminado correctamente" });
        }

    }
}
