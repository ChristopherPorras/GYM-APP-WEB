using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class DescuentosMapper : IObjectMapper, ICrudStatements
    {
        public BaseClass BuildObject(Dictionary<string, object> row)
        {
            var descuento = new Descuentos
            {
                ID = (int)row["Id"],
                Nombre = row["Nombre"].ToString(),
                Descripcion = row["Descripcion"].ToString(),
                Porcentaje = (decimal)row["Porcentaje"],
                FechaInicio = (DateTime)row["FechaInicio"],
                FechaFin = (DateTime)row["FechaFin"]
            };

            return descuento;
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
            var descuento = (Descuentos)entity;
            var operation = new SqlOperation { ProcedureName = "sp_CreateDescuento" };

            operation.AddVarcharParam("Nombre", descuento.Nombre);
            operation.AddVarcharParam("Descripcion", descuento.Descripcion);
            operation.AddDecimalParam("Porcentaje", descuento.Porcentaje);
            operation.AddDateTimeParam("FechaInicio", descuento.FechaInicio);
            operation.AddDateTimeParam("FechaFin", descuento.FechaFin);

            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseClass entity)
        {
            var descuento = (Descuentos)entity;
            var operation = new SqlOperation { ProcedureName = "sp_UpdateDescuento" };

            operation.AddIntegerParam("Id", descuento.ID);
            operation.AddVarcharParam("Nombre", descuento.Nombre);
            operation.AddVarcharParam("Descripcion", descuento.Descripcion);
            operation.AddDecimalParam("Porcentaje", descuento.Porcentaje);
            operation.AddDateTimeParam("FechaInicio", descuento.FechaInicio);
            operation.AddDateTimeParam("FechaFin", descuento.FechaFin);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseClass entity)
        {
            var descuento = (Descuentos)entity;
            var operation = new SqlOperation { ProcedureName = "sp_DeleteDescuento" };

            operation.AddIntegerParam("Id", descuento.ID);

            return operation;
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "sp_GetDescuentoById" };
            operation.AddIntegerParam("Id", id);

            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "sp_GetAllDescuentos" };
            return operation;
        }
    }
}
