using DTO;
using DataAccess.DAO;
using System;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class RoleMapper : EntityMapper
    {
        public override SqlOperation GetCreateStatement(BaseClass entity)
        {
            var role = (Role)entity;
            var operation = new SqlOperation
            {
                ProcedureName = "CreateRole"
            };
            operation.AddVarcharParam("Nombre", role.Nombre);
            operation.AddVarcharParam("Descripcion", role.Descripcion);
            return operation;
        }

        public override SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetRoleById"
            };
            operation.AddIntegerParam("Id", id);
            return operation;
        }

        public override SqlOperation GetRetrieveAllStatement()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetAllRoles"
            };
            return operation;
        }

        public override SqlOperation GetUpdateStatement(BaseClass entity)
        {
            var role = (Role)entity;
            var operation = new SqlOperation
            {
                ProcedureName = "UpdateRole"
            };
            operation.AddIntegerParam("Id", role.ID);
            operation.AddVarcharParam("Nombre", role.Nombre);
            operation.AddVarcharParam("Descripcion", role.Descripcion);
            return operation;
        }

        public override SqlOperation GetDeleteStatement(BaseClass entity)
        {
            var role = (Role)entity;
            var operation = new SqlOperation
            {
                ProcedureName = "DeleteRole"
            };
            operation.AddIntegerParam("Id", role.ID);
            return operation;
        }

        public override BaseClass BuildObject(Dictionary<string, object> row)
        {
            var role = new Role
            {
                ID = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString(),
                Descripcion = row["Descripcion"].ToString()
            };
            return role;
        }

        public override List<BaseClass> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<BaseClass>();

            foreach (var row in lstRows)
            {
                var role = BuildObject(row);
                lstResults.Add(role);
            }
            return lstResults;
        }
    }
}
