using Microsoft.AspNetCore.Mvc;
using ProyectoPOO.Data;
using ProyectoPOO.Models;

namespace ProyectoPOO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrupoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GrupoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Grupo grupo)
        {
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();
            return Ok(grupo);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            return Ok(_context.Grupos.ToList());
        }
    }
}
