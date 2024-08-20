using Microsoft.AspNetCore.Mvc;
using BL;
using DTO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager _manager;

        public RoleController()
        {
            _manager = new RoleManager();
        }

        [HttpGet]
        public ActionResult<List<Role>> GetAllRoles()
        {
            var roles = _manager.GetAllRoles();
            return Ok(roles);
        }
    }
}
