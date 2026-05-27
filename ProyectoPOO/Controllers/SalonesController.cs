using Microsoft.AspNetCore.Mvc;
using ProyectoPOO.Data;
using ProyectoPOO.Models;

namespace ProyectoPOO.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class SalonesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalonesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Salon salon)
        {
            _context.Salones.Add(salon);
            await _context.SaveChangesAsync();
            return Ok(salon);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            return Ok(_context.Salones.ToList());
        }
    }

    
    
}
