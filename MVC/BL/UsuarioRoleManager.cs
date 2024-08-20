using DataAccess.CRUD;
using DTO;
using System;
using System.Collections.Generic;

namespace BL
{
    public class UsuarioRoleManager
    {
        private readonly UsuarioRoleCrudFactory _usuarioRoleCrudFactory;
        private readonly RoleCrudFactory _roleCrudFactory;

        public UsuarioRoleManager()
        {
            _usuarioRoleCrudFactory = new UsuarioRoleCrudFactory();
            _roleCrudFactory = new RoleCrudFactory();
        }

        public void CreateUsuarioConRol(UsuarioRole usuarioRole)
        {
            _usuarioRoleCrudFactory.Create(usuarioRole);
        }

        public UsuarioRole GetUsuarioRoleByEmail(string correoElectronico)
        {
            return _usuarioRoleCrudFactory.RetrieveByEmail(correoElectronico);
        }

        public List<UsuarioRole> GetAllUsuariosConRoles()
        {
            var usuariosRoles = _usuarioRoleCrudFactory.RetrieveAll<UsuarioRole>();
            return usuariosRoles;
        }

        public void UpdateUsuarioRole(UsuarioRoleUpdate usuarioRole)
        {
            _usuarioRoleCrudFactory.Update(usuarioRole);
        }

        public void DeleteUsuarioRole(UsuarioRole usuarioRole)
        {
            _usuarioRoleCrudFactory.Delete(usuarioRole);
        }
    }
}
