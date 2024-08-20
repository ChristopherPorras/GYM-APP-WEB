using Microsoft.AspNetCore.Mvc;
using BL;
using DTO;
using System.Collections.Generic;
using DTO.Rutinas;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly EquipoManager manager;

        public EquipoController()
        {
            manager = new EquipoManager();
        }

        [HttpPost("sp_CrearEquipo")]
        public ActionResult CreateEquipo([FromBody] Equipo equipo)
        {
            manager.CreateEquipo(equipo);
            return Ok();
        }

        [HttpGet("sp_ObtenerTodosLosEquipos")]
        public ActionResult<List<Equipo>> GetEquipos()
        {
            var equipos = manager.GetEquipos();
            return Ok(equipos);
        }

        [HttpGet("sp_ObtenerEquipoPorId/{equipoId}")]
        public ActionResult<Equipo> GetEquipoById(int equipoId)
        {
            var equipo = manager.GetEquipoById(equipoId);
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPut("sp_ActualizarEquipo/{equipoId}")]
        public ActionResult UpdateEquipo(int equipoId, [FromBody] Equipo equipo)
        {
            var existingEquipo = manager.GetEquipoById(equipoId);
            if (existingEquipo == null)
            {
                return NotFound();
            }

            equipo.EquipoId = equipoId;
            manager.UpdateEquipo(equipo);
            return Ok();
        }

        [HttpDelete("sp_EliminarEquipo/{equipoId}")]
        public ActionResult DeleteEquipo(int equipoId)
        {
            var existingEquipo = manager.GetEquipoById(equipoId);
            if (existingEquipo == null)
            {
                return NotFound();
            }

            manager.DeleteEquipo(equipoId);
            return Ok();
        }
    }
}
