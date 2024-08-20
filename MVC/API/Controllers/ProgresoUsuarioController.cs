using BL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgresoUsuarioController : ControllerBase
    {
        private readonly ProgresoUsuarioManager progresoManager;

        public ProgresoUsuarioController()
        {
            progresoManager = new ProgresoUsuarioManager();
        }

        [HttpGet("ObtenerProgreso/{correo}")]
        public ActionResult<List<ProgresoUsuarioDTO>> ObtenerProgreso(string correo)
        {
            var progreso = progresoManager.ObtenerProgresoPorCorreo(correo);
            return Ok(progreso);
        }

        [HttpPost("CrearProgreso")]
        public IActionResult CrearProgreso([FromBody] ProgresoUsuarioDTO progreso)
        {
            if (ModelState.IsValid)
            {
                progresoManager.CrearProgreso(progreso);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("ActualizarProgreso")]
        public IActionResult ActualizarProgreso([FromBody] ProgresoUsuarioDTO progreso)
        {
            if (ModelState.IsValid)
            {
                progresoManager.ActualizarProgreso(progreso);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("EliminarProgreso/{id}")]
        public IActionResult EliminarProgreso(int id)
        {
            progresoManager.EliminarProgreso(id);
            return Ok();
        }
    }
}
