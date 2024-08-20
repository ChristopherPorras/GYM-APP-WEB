using DataAccess.CRUD;
using DTO;
using System.Collections.Generic;

namespace BL
{
    public class RoleManager
    {
        private readonly RoleCrudFactory _crudFactory;

        public RoleManager()
        {
            _crudFactory = new RoleCrudFactory();
        }

        public List<Role> GetAllRoles()
        {
            return _crudFactory.RetrieveAll<Role>();
        }
    }
}
