using Microsoft.AspNetCore.Mvc;
using BL;
using DTO;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsuarioRoleController : ControllerBase
    {
        private readonly UsuarioRoleManager _usuarioRoleManager;

        public UsuarioRoleController()
        {
            _usuarioRoleManager = new UsuarioRoleManager();
        }

        [HttpGet]
        public ActionResult<List<UsuarioRole>> GetAllUsuariosConRoles()
        {
            var usuariosRoles = _usuarioRoleManager.GetAllUsuariosConRoles();
            return Ok(usuariosRoles);
        }

        [HttpGet("{correoElectronico}")]
        public ActionResult<UsuarioRole> GetUsuarioRoleByEmail(string correoElectronico)
        {
            var usuarioRole = _usuarioRoleManager.GetUsuarioRoleByEmail(correoElectronico);
            if (usuarioRole == null)
                return NotFound();
            return Ok(usuarioRole);
        }

        [HttpPost]
        public IActionResult CreateUsuarioConRol([FromBody] UsuarioRole usuarioRole)
        {
            try
            {
                if (usuarioRole == null)
                {
                    return BadRequest("UsuarioRole object is null");
                }

                // Validaciones adicionales
                if (string.IsNullOrEmpty(usuarioRole.CorreoElectronico) ||
                    string.IsNullOrEmpty(usuarioRole.Nombre) ||
                    string.IsNullOrEmpty(usuarioRole.Contrasena) ||
                    usuarioRole.RolId <= 0)
                {
                    return BadRequest("Missing required fields");
                }

                _usuarioRoleManager.CreateUsuarioConRol(usuarioRole);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPut]
        public IActionResult UpdateUsuarioRole([FromBody] UsuarioRoleUpdate usuarioRole)
        {
            try
            {
                _usuarioRoleManager.UpdateUsuarioRole(usuarioRole);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{correoElectronico}")]
        public IActionResult DeleteUsuarioRole(string correoElectronico)
        {
            try
            {
                _usuarioRoleManager.DeleteUsuarioRole(new UsuarioRole { CorreoElectronico = correoElectronico });
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}