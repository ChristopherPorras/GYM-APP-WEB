using Microsoft.AspNetCore.Mvc;
using BL;
using DTO;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedidasCorporalesController : ControllerBase
    {
        private readonly MedidasCorporalesManager _manager;

        public MedidasCorporalesController()
        {
            _manager = new MedidasCorporalesManager();
        }

        // GET: api/MedidasCorporales/ObtenerMedidasCorporales
        /// <summary>
        /// Ejecuta el procedimiento almacenado 'ObtenerMedidasCorporales' para obtener todas las medidas.
        /// </summary>
        [HttpGet("ObtenerMedidasCorporales")]
        public ActionResult<IEnumerable<MedidasCorporales>> ObtenerMedidasCorporales()
        {
            var medidas = _manager.GetMedidasCorporales();
            return Ok(medidas);
        }

        // GET: api/MedidasCorporales/ObtenerMedidasCorporalesPorId/5
        /// <summary>
        /// Ejecuta el procedimiento almacenado 'ObtenerMedidasCorporalesPorId' para obtener una medida por su Id.
        /// </summary>
        [HttpGet("ObtenerMedidasCorporalesPorId/{id}")]
        public ActionResult<MedidasCorporales> ObtenerMedidasCorporalesPorId(int id)
        {
            var medida = _manager.GetMedidaCorporalById(id);

            if (medida == null)
            {
                return NotFound();
            }

            return Ok(medida);
        }

        // GET: api/MedidasCorporales/ObtenerMedidasPorCorreo/{correoElectronico}
        /// <summary>
        /// Ejecuta el procedimiento almacenado 'ObtenerMedidasPorCorreo' para obtener una medida por el correo electrónico.
        /// </summary>
        [HttpGet("ObtenerMedidasPorCorreo/{correoElectronico}")]
        public ActionResult<MedidasCorporales> ObtenerMedidasPorCorreo(string correoElectronico)
        {
            var medida = _manager.GetMedidaCorporalByCorreo(correoElectronico);

            if (medida == null)
            {
                return NotFound();
            }

            return Ok(medida);
        }

        // POST: api/MedidasCorporales/InsertarMedidaCorporal
        /// <summary>
        /// Ejecuta el procedimiento almacenado 'InsertarMedidaCorporal' para crear una nueva medida corporal.
        /// </summary>
        [HttpPost("InsertarMedidaCorporal")]
        public IActionResult InsertarMedidaCorporal([FromBody] MedidasCorporales medida)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _manager.CreateMedidaCorporal(medida);

            return CreatedAtAction(nameof(ObtenerMedidasCorporalesPorId), new { id = medida.MedidasId }, medida);
        }

        // PUT: api/MedidasCorporales/ActualizarMedidaCorporal/5
        /// <summary>
        /// Ejecuta el procedimiento almacenado 'ActualizarMedidaCorporal' para actualizar una medida existente.
        /// </summary>
        [HttpPut("ActualizarMedidaCorporal/{id}")]
        public IActionResult ActualizarMedidaCorporal(int id, [FromBody] MedidasCorporales medida)
        {
            if (id != medida.MedidasId)
            {
                return BadRequest();
            }

            var existingMedida = _manager.GetMedidaCorporalById(id);
            if (existingMedida == null)
            {
                return NotFound();
            }

            _manager.UpdateMedidaCorporal(medida);

            return NoContent();
        }

        // DELETE: api/MedidasCorporales/EliminarMedidaCorporal/5
        /// <summary>
        /// Ejecuta el procedimiento almacenado 'EliminarMedidaCorporal' para eliminar una medida corporal por su Id.
        /// </summary>
        [HttpDelete("EliminarMedidaCorporal/{id}")]
        public IActionResult EliminarMedidaCorporal(int id)
        {
            var medida = _manager.GetMedidaCorporalById(id);
            if (medida == null)
            {
                return NotFound();
            }

            _manager.DeleteMedidaCorporal(id);

            return NoContent();
        }
    }
}
