using GestionSalones.Data;
using GestionSalones.DTOs;
using GestionSalones.Helpers;
using GestionSalones.Models;
using GestionSalones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionSalones.Controllers
{

    [ApiController]              
    [Route("api/[controller]")]  
    public class AuthController : ControllerBase  
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // =========================================================
        // ✅ LOGIN
        // POST: api/auth/login
        // =========================================================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            // ✅ Validar datos vacíos
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email y contraseña son obligatorios");
            }

            // ✅ Normalizar email
            var email = dto.Email.Trim().ToLower();

            // ✅ Buscar usuario
            var user = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            // ✅ Validar existencia
            if (user == null)
                return Unauthorized("Usuario inválido");

            // ✅ Validar activo
            if (!user.Activo)
                return Unauthorized("El usuario está desactivado");

            // ✅ Validar contraseña
            bool passwordCorrecta =
                BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!passwordCorrecta)
                return Unauthorized("Contraseña incorrecta");

            // ✅ Obtener roles
            var roles = user.UsuarioRoles
                .Select(ur => ur.Rol.Nombre)
                .ToList();

            // ✅ Generar token
            var token = _jwtService.GenerateToken(user, roles);

            // ✅ Respuesta limpia
            return Ok(new
            {
                token,
                usuario = new
                {
                    user.Id,
                    user.Nombre,
                    user.Email,
                    Roles = roles
                }
            });
        }

        // =========================================================
        // ✅ REGISTER
        // POST: api/auth/register
        // =========================================================
        [HttpPost("register")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo)]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            // ✅ Validaciones básicas
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("El email es obligatorio");

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("La contraseña es obligatoria");

            if (dto.Password.Length < 6)
                return BadRequest("La contraseña debe tener mínimo 6 caracteres");

            // ✅ Normalizar email
            var email = dto.Email.Trim().ToLower();

            // ✅ Verificar duplicado
            var existe = await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == email);

            if (existe)
                return BadRequest("Ya existe un usuario con ese email");

            // ✅ Verificar rol válido
            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Nombre == dto.Rol);

            if (rol == null)
                return BadRequest("El rol especificado no existe");

            // ✅ Crear usuario
            var usuario = new Usuarios
            {
                Nombre = dto.Nombre.Trim(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // ✅ Asignar rol
            _context.UsuarioRoles.Add(new UsuarioRol
            {
                UsuarioId = usuario.Id,
                RolId = rol.Id
            });

            await _context.SaveChangesAsync();

            // =====================================================
            // ✅ Crear entidades especiales automáticamente
            // =====================================================

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
                    UsuarioId = usuario.Id,
                    CarreraId = dto.CarreraId //<--- cambio de ultimo momento para asignar carrera al estudiante
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario registrado correctamente",
                usuario = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Email,
                    Rol = rol.Nombre
                }
            });
        }

        // =========================================================
        // ✅ PERFIL DEL USUARIO ACTUAL
        // GET: api/auth/me
        // =========================================================
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> MiPerfil()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized();

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);

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
        // ✅ DESACTIVAR USUARIO
        // PUT: api/auth/desactivar/5
        // =========================================================
        [HttpPut("desactivar/{id}")]
        [Authorize(Roles = Roles.Admin)]
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
        // ✅ ACTIVAR USUARIO
        // PUT: api/auth/activar/5
        // =========================================================
        [HttpPut("activar/{id}")]
        [Authorize(Roles = Roles.Admin)]
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
        // ⚠️ TEMPORAL - SOLO DESARROLLO
        // POST: api/auth/seed
        // =========================================================
        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            // =====================================================
            // ✅ CREAR ROLES
            // =====================================================

            if (!_context.Roles.Any())
            {
                _context.Roles.AddRange(
                    new Rol { Nombre = Roles.Admin },
                    new Rol { Nombre = Roles.Administrativo },
                    new Rol { Nombre = Roles.Docente },
                    new Rol { Nombre = Roles.Estudiante }
                );

                await _context.SaveChangesAsync();
            }

            // =====================================================
            // ✅ CREAR ADMIN
            // =====================================================

            var adminExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == "admin@test.com");

            if (!adminExiste)
            {
                var admin = new Usuarios
                {
                    Nombre = "Administrador",
                    Email = "admin@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Activo = true
                };

                _context.Usuarios.Add(admin);
                await _context.SaveChangesAsync();

                var rolAdmin = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Nombre == Roles.Admin);

                if (rolAdmin != null)
                {
                    _context.UsuarioRoles.Add(new UsuarioRol
                    {
                        UsuarioId = admin.Id,
                        RolId = rolAdmin.Id
                    });

                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new
            {
                message = "Datos iniciales creados correctamente",
                admin = new
                {
                    email = "admin@test.com",
                    password = "admin123"
                }
            });
        }
    }
}
