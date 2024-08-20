using BL;
using DTO;
using DTO.Rutinas;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EjercicioController : ControllerBase
    {
        private readonly EjercicioManager manager;

        public EjercicioController()
        {
            manager = new EjercicioManager();
        }

        [HttpPost("sp_RegistrarEjercicio")]
        public ActionResult CreateEjercicio([FromBody] Ejercicio ejercicio)
        {
            manager.CreateEjercicio(ejercicio);
            return Ok();
        }

        [HttpGet("sp_ObtenerTodosLosEjercicios")]
        public ActionResult<List<Ejercicio>> GetEjercicios()
        {
            var ejercicios = manager.GetEjercicios();
            return Ok(ejercicios);
        }

        [HttpGet("sp_ObtenerEjercicioPorId/{ejercicioId}")]
        public ActionResult<Ejercicio> GetEjercicioById(int ejercicioId)
        {
            var ejercicio = manager.GetEjercicioById(ejercicioId);
            if (ejercicio == null)
            {
                return NotFound();
            }
            return Ok(ejercicio);
        }

        [HttpPut("sp_ActualizarEjercicio/{ejercicioId}")]
        public ActionResult UpdateEjercicio(int ejercicioId, [FromBody] Ejercicio ejercicio)
        {
            var existingEjercicio = manager.GetEjercicioById(ejercicioId);
            if (existingEjercicio == null)
            {
                return NotFound();
            }

            ejercicio.EjercicioId = ejercicioId;  // Ensure the ID is set correctly
            manager.UpdateEjercicio(ejercicio);
            return Ok();
        }

        [HttpDelete("sp_EliminarEjercicio/{ejercicioId}")]
        public ActionResult DeleteEjercicio(int ejercicioId)
        {
            var existingEjercicio = manager.GetEjercicioById(ejercicioId);
            if (existingEjercicio == null)
            {
                return NotFound();
            }

            manager.DeleteEjercicio(ejercicioId);
            return Ok();
        }
    }
}
