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

    public class HorariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HorariosController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/horarios
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetHorarios()
        {
            var horarios = await _context.Horarios
                .AsNoTracking()
                .Select(h => new HorarioDTO
                {
                    Id = h.Id,
                    DiaSemana = h.DiaSemana,
                    HoraInicio = h.HoraInicio,
                    HoraFin = h.HoraFin
                })
                .ToListAsync<HorarioDTO>();

            return Ok(horarios.Select(h => new
            {
                h.Id,
                Dia = NombreDia(h.DiaSemana),
                HoraInicio = h.HoraInicio.ToString(@"hh\:mm"),
                HoraFin = h.HoraFin.ToString(@"hh\:mm")
            }));
        }

        // ✅ GET: api/horarios/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Administrativo + "," + Roles.Docente + "," + Roles.Estudiante)]
        public async Task<IActionResult> GetHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);

            if (horario == null)
                return NotFound("Horario no encontrado");

            return Ok(new
            {
                horario.Id,
                Dia = NombreDia(horario.DiaSemana),
                HoraInicio = horario.HoraInicio.ToString(@"hh\:mm"),
                HoraFin = horario.HoraFin.ToString(@"hh\:mm")
            });
        }

        // ✅ POST: api/horarios
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CrearHorario(Horario horario)
        {
            // Validar día
            if (horario.DiaSemana < 1 || horario.DiaSemana > 7)
                return BadRequest("El día debe estar entre 1 (Lunes) y 7 (Domingo)");

            // Validar rango interno del horario
            if (horario.HoraFin <= horario.HoraInicio)
                return BadRequest("La hora de fin debe ser mayor a la hora de inicio");


            // ✅ Validar rango institucional 6 AM - 10 PM
            if (horario.HoraInicio < HoraMinimaInstitucional ||
                horario.HoraFin > HoraMaximaInstitucional)
            {
                return BadRequest(
                    "Los horarios solo pueden estar entre 06:00 y 22:00");
            }


            // ✅ Validar solapamiento
            var haySolapamiento = await _context.Horarios.AnyAsync(h =>
                h.DiaSemana == horario.DiaSemana &&
                horario.HoraInicio < h.HoraFin &&
                horario.HoraFin > h.HoraInicio
            );

            if (haySolapamiento)
                return BadRequest(
                    "El horario se cruza con otro bloque ya existente");


            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHorario), new { id = horario.Id }, new
            {
                horario.Id,
                Dia = NombreDia(horario.DiaSemana),
                HoraInicio = horario.HoraInicio.ToString(@"hh\:mm"),
                HoraFin = horario.HoraFin.ToString(@"hh\:mm")
            });
        }

        // ✅ PUT: api/horarios/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> EditarHorario(int id, Horario horarioEditado)
        {
            var horario = await _context.Horarios.FindAsync(id);

            if (horario == null)
                return NotFound("Horario no encontrado");


            if (horarioEditado.DiaSemana < 1 || horarioEditado.DiaSemana > 7)
                return BadRequest("El día debe estar entre 1 y 7");


            if (horarioEditado.HoraFin <= horarioEditado.HoraInicio)
                return BadRequest("La hora de fin debe ser mayor a la hora de inicio");


            // ✅ Rango institucional
            if (horarioEditado.HoraInicio < HoraMinimaInstitucional ||
                horarioEditado.HoraFin > HoraMaximaInstitucional)
            {
                return BadRequest(
                    "Los horarios solo pueden estar entre 06:00 y 22:00");
            }


            // ✅ Validar solapamiento excluyendo el mismo horario
            var haySolapamiento = await _context.Horarios.AnyAsync(h =>
                h.Id != id &&
                h.DiaSemana == horarioEditado.DiaSemana &&
                horarioEditado.HoraInicio < h.HoraFin &&
                horarioEditado.HoraFin > h.HoraInicio
            );

            if (haySolapamiento)
                return BadRequest(
                    "El horario se cruza con otro bloque existente");


            horario.DiaSemana = horarioEditado.DiaSemana;
            horario.HoraInicio = horarioEditado.HoraInicio;
            horario.HoraFin = horarioEditado.HoraFin;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                horario.Id,
                Dia = NombreDia(horario.DiaSemana),
                HoraInicio = horario.HoraInicio.ToString(@"hh\:mm"),
                HoraFin = horario.HoraFin.ToString(@"hh\:mm")
            });
        }

        // ✅ DELETE: api/horarios/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> EliminarHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);

            if (horario == null)
                return NotFound("Horario no encontrado");

            // Verificar si tiene asignaciones activas
            var tieneAsignaciones = await _context.Asignaciones
                .AnyAsync(a => a.HorarioId == id);

            if (tieneAsignaciones)
                return BadRequest("No se puede eliminar el horario porque tiene asignaciones activas");

            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Horario eliminado correctamente" });
        }

        // 🔧 Método auxiliar para convertir número de día a nombre
        private static string NombreDia(int dia) => dia switch
        {
            1 => "Lunes",
            2 => "Martes",
            3 => "Miércoles",
            4 => "Jueves",
            5 => "Viernes",
            6 => "Sábado",
            7 => "Domingo",
            _ => "Desconocido"
        };

        private static readonly TimeSpan HoraMinimaInstitucional = new(6, 0, 0);   // 6:00 AM
        private static readonly TimeSpan HoraMaximaInstitucional = new(22, 0, 0);  // 10:00 PM
    }
}
