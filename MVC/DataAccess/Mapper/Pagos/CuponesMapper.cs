using System.Collections.Generic;
using DataAccess.DAO;
using DTO;

namespace DataAccess.Mapper
{
    public class CuponesMapper : IObjectMapper, ICrudStatements
    {
        public BaseClass BuildObject(Dictionary<string, object> row)
        {
            var cupon = new Cupones
            {
                Id = (int)row["Id"],
                Codigo = row["Codigo"].ToString(),
                DescuentoId = (int)row["DescuentoId"],
                FechaCreacion = (DateTime)row["FechaCreacion"],
                Usado = (bool)row["Usado"]
            };

            return cupon;
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
            var cupon = (Cupones)entity;
            var operation = new SqlOperation { ProcedureName = "sp_CrearCupon" };

            operation.AddVarcharParam("Codigo", cupon.Codigo);
            operation.AddIntegerParam("DescuentoId", cupon.DescuentoId);

            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseClass entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetDeleteStatement(BaseClass entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetApplyCouponStatement(string codigo, string correoElectronico)
        {
            var operation = new SqlOperation { ProcedureName = "sp_AplicarCupon" };
            operation.AddVarcharParam("Codigo", codigo);
            operation.AddVarcharParam("CorreoElectronico", correoElectronico);

            return operation;
        }
    }
}
