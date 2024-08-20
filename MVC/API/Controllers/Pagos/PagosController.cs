using BL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly PagosManager manager;

        public PagosController()
        {
            manager = new PagosManager();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePago(Pagos pago)
        {
            try
            {
                manager.CreatePago(pago);
                return Ok(new { message = "Compra realizada con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al procesar el pago: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<Pagos>> GetAllPagos()
        {
            var pagos = manager.RetrieveAllPagos();
            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public ActionResult<Pagos> GetPagoById(int id)
        {
            var pago = manager.RetrievePagoById(id);
            if (pago == null)
                return NotFound();

            return Ok(pago);
        }

        [HttpPut]
        public ActionResult UpdatePago(Pagos pago)
        {
            manager.UpdatePago(pago);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePago(int id)
        {
            manager.DeletePago(id);
            return Ok();
        }
    }
}
