using DataAccess.DAO;
using DTO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class PagosMapper : IObjectMapper, ICrudStatements
    {
        public BaseClass BuildObject(Dictionary<string, object> row)
        {
            var pago = new Pagos
            {
                ID = (int)row["Id"],
                CorreoElectronico = row["CorreoElectronico"].ToString(),
                FechaPago = (DateTime)row["FechaPago"],
                Monto = (decimal)row["Monto"],
                MetodoPago = row["MetodoPago"].ToString(),
                EstadoPago = row["EstadoPago"].ToString()
            };

            return pago;
        }

        public List<BaseClass> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<BaseClass>();

            foreach (var row in lstRows)
            {
                var obj = BuildObject(row);
                lstResults.Add(obj);
            }

            return lstResults;
        }

        public SqlOperation GetCreateStatement(BaseClass entity)
        {
            var pago = (Pagos)entity;
            var operation = new SqlOperation { ProcedureName = "sp_CreatePago" };

            operation.AddVarcharParam("CorreoElectronico", pago.CorreoElectronico);
            operation.AddDateTimeParam("FechaPago", pago.FechaPago);
            operation.AddDecimalParam("Monto", pago.Monto);
            operation.AddVarcharParam("MetodoPago", pago.MetodoPago);
            operation.AddVarcharParam("EstadoPago", pago.EstadoPago);

            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseClass entity)
        {
            var pago = (Pagos)entity;
            var operation = new SqlOperation { ProcedureName = "sp_UpdatePago" };

            operation.AddIntegerParam("Id", pago.ID);
            operation.AddVarcharParam("CorreoElectronico", pago.CorreoElectronico);
            operation.AddDateTimeParam("FechaPago", pago.FechaPago);
            operation.AddDecimalParam("Monto", pago.Monto);
            operation.AddVarcharParam("MetodoPago", pago.MetodoPago);
            operation.AddVarcharParam("EstadoPago", pago.EstadoPago);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseClass entity)
        {
            var pago = (Pagos)entity;
            var operation = new SqlOperation { ProcedureName = "sp_DeletePago" };

            operation.AddIntegerParam("Id", pago.ID);

            return operation;
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "sp_GetPagoById" };
            operation.AddIntegerParam("Id", id);

            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "sp_GetAllPagos" };
            return operation;
        }
    }
}
