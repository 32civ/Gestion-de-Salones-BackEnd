using Microsoft.AspNetCore.Mvc;
using ProyectoPOO.Data;
using ProyectoPOO.Models;
using ProyectoPOO.Services;

namespace ProyectoPOO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsignacionesController : ControllerBase
    {
        private readonly AsignacionService _service;

        public AsignacionesController(AsignacionService service)
        {
            _service = service;
        }

        [HttpPost("auto")]
        public async Task<IActionResult> AsignarAutomatico(int grupoId, int horarioId)
        {
            var resultado = await _service.AsignarSalon(grupoId, horarioId);
            return Ok(resultado);
        }
    }
}
