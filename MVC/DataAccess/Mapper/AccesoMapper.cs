using DataAccess.DAO;
using DTO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class AccesoMapper : ICrudStatements, IObjectMapper
    {
        public List<BaseClass> BuildObjects(List<Dictionary<string, object>> objectRows)
        {
            var accesoList = new List<BaseClass>();

            foreach (var row in objectRows)
            {
                var acceso = BuildObject(row);
                accesoList.Add(acceso);
            }

            return accesoList;
        }

        public BaseClass BuildObject(Dictionary<string, object> result)
        {
            var acceso = new Acceso
            {
                ID = int.Parse(result["Id"].ToString()),
                Nombre = result["Nombre"].ToString()
            };

            return acceso;
        }

        public SqlOperation GetCreateStatement(BaseClass entityDTO)
        {
            var operation = new SqlOperation { ProcedureName = "CreateAcceso" };

            var acceso = (Acceso)entityDTO;
            operation.AddVarcharParam("Nombre", acceso.Nombre);

            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseClass entityDTO)
        {
            var operation = new SqlOperation { ProcedureName = "UpdateAcceso" };

            var acceso = (Acceso)entityDTO;
            operation.AddIntegerParam("ID", acceso.ID);
            operation.AddVarcharParam("Nombre", acceso.Nombre);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseClass entityDTO)
        {
            var operation = new SqlOperation { ProcedureName = "DeleteAcceso" };

            var acceso = (Acceso)entityDTO;
            operation.AddIntegerParam("ID", acceso.ID);

            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            return new SqlOperation { ProcedureName = "GetAllAccesos" };
        }

        public SqlOperation GetRetrieveByIdStatement(int Id)
        {
            var operation = new SqlOperation { ProcedureName = "GetAccesoById" };
            operation.AddIntegerParam("Id", Id);
            return operation;
        }
    }
}
