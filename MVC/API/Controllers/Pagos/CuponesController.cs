using BL;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponesController : ControllerBase
    {
        private readonly CuponesManager _manager;

        public CuponesController()
        {
            _manager = new CuponesManager();
        }

        [HttpPost]
        public ActionResult CreateCupon(Cupones cupon)
        {
            _manager.CreateCupon(cupon);
            return Ok();
        }

        [HttpPost("apply")]
        public ActionResult ApplyCupon(string codigo, string correoElectronico)
        {
            _manager.ApplyCupon(codigo, correoElectronico);
            return Ok();
        }
    }
}
