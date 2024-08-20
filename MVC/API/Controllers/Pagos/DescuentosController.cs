using BL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DescuentosController : ControllerBase
    {
        private readonly DescuentosManager manager;

        public DescuentosController()
        {
            manager = new DescuentosManager();
        }

        [HttpPost]
        public ActionResult CreateDescuento(Descuentos descuento)
        {
            manager.CreateDescuento(descuento);
            return Ok();
        }

        [HttpGet]
        public ActionResult<List<Descuentos>> GetAllDescuentos()
        {
            var descuentos = manager.RetrieveAllDescuentos();
            return Ok(descuentos);
        }

        [HttpGet("{id}")]
        public ActionResult<Descuentos> GetDescuentoById(int id)
        {
            var descuento = manager.RetrieveDescuentoById(id);
            if (descuento == null)
                return NotFound();

            return Ok(descuento);
        }

        [HttpPut]
        public ActionResult UpdateDescuento(Descuentos descuento)
        {
            manager.UpdateDescuento(descuento);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDescuento(int id)
        {
            manager.DeleteDescuento(id);
            return Ok();
        }
    }
}
