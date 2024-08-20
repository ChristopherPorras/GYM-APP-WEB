using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL;
using DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClasesGrupalesController : ControllerBase
    {
        private readonly ClaseGrupalManager _manager;

        public ClasesGrupalesController()
        {
            _manager = new ClaseGrupalManager();
        }

        // GET: api/ClasesGrupales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaseGrupal>>> GetClasesGrupales()
        {
            var clases = await _manager.GetClasesGrupales();
            return Ok(clases);
        }

        // GET: api/ClasesGrupales/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaseGrupal>> GetClaseGrupalById(int id)
        {
            var clase = await _manager.GetClaseGrupalById(id);

            if (clase == null)
            {
                return NotFound();
            }

            return Ok(clase);
        }

        // GET: api/ClasesGrupales/ByNombre/{nombre}
        [HttpGet("ByNombre/{nombre}")]
        public async Task<ActionResult<IEnumerable<ClaseGrupal>>> ClasesGrupalesPorNombre(string nombre)
        {
            var clases = await _manager.ClasesGrupalesPorNombre(nombre);

            if (clases == null || clases.Count == 0)
            {
                return NotFound("No se encontraron clases con ese nombre.");
            }

            return Ok(clases);
        }

        // POST: api/ClasesGrupales
        [HttpPost]
        public async Task<ActionResult> PostClaseGrupal([FromBody] ClaseGrupal claseGrupal)
        {
            if (claseGrupal == null)
            {
                return BadRequest("La clase grupal no puede ser nula.");
            }

            try
            {
                await _manager.CreateClaseGrupal(claseGrupal);
                return CreatedAtAction(nameof(GetClaseGrupalById), new { id = claseGrupal.ClaseGrupalID }, claseGrupal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al crear la clase grupal.", details = ex.Message });
            }
        }

        // PUT: api/ClasesGrupales/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaseGrupal(int id, [FromBody] ClaseGrupal claseGrupal)
        {
            if (id != claseGrupal.ClaseGrupalID || claseGrupal == null)
            {
                return BadRequest("Los datos de la clase grupal son inválidos.");
            }

            try
            {
                await _manager.UpdateClaseGrupal(claseGrupal);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al actualizar la clase grupal.", details = ex.Message });
            }
        }

        // DELETE: api/ClasesGrupales/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaseGrupal(int id)
        {
            try
            {
                await _manager.DeleteClaseGrupal(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al eliminar la clase grupal.", details = ex.Message });
            }
        }
    }
}
