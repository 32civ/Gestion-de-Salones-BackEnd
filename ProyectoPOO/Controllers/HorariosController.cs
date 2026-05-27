using Microsoft.AspNetCore.Mvc;
using ProyectoPOO.Data;
using ProyectoPOO.Models;

namespace ProyectoPOO.Controllers
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

        [HttpPost]
        public async Task<IActionResult> Crear(Horario horario)
        {
            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();
            return Ok(horario);
        }

        [HttpGet]
        public IActionResult Obtener()
        {
            return Ok(_context.Horarios.ToList());
        }
    }
}
