using Microsoft.AspNetCore.Mvc;
using BL;
using DTO;
using System.Collections.Generic;
using DataAccess.DAO;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly AccesoManager manager;

        public AccessController()
        {
            manager = new AccesoManager();
        }

        [HttpPost("create")]
        public ActionResult CreateAcceso(Acceso acceso)
        {
            manager.CreateAcceso(acceso);
            return Ok();
        }

        [HttpGet]
        public ActionResult<List<Acceso>> GetAllAccesos()
        {
            var accesos = manager.GetAllAccesos();
            return Ok(accesos);
        }

        [HttpPost("assign")]
        public ActionResult AssignAccessToRol(int rolID, int accesoID)
        {
            manager.AssignAccessToRol(rolID, accesoID);
            return Ok();
        }

        [HttpPost("remove")]
        public ActionResult RemoveAccessFromRol(int rolID, int accesoID)
        {
            manager.RemoveAccessFromRol(rolID, accesoID);
            return Ok();
        }

        [HttpGet("{rolID}")]
        public ActionResult<List<Acceso>> GetAccessByRol(int rolID)
        {
            var accesos = manager.GetAccessByRol(rolID);
            return Ok(accesos);
        }
    }
}
