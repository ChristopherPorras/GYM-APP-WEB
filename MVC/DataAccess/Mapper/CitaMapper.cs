using DTO;
using DataAccess.DAO;
using System;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class CitaMapper : IObjectMapper, ICrudStatements
    {
        public BaseClass BuildObject(Dictionary<string, object> row)
        {
            if (row == null)
            {
                return null; // o maneja el caso de manera diferente según sea necesario
            }

            var cita = new Cita
            {
                ID = row.ContainsKey("Id") ? Convert.ToInt32(row["Id"]) : 0,
                CorreoElectronico = row.ContainsKey("CorreoElectronico") ? row["CorreoElectronico"].ToString() : string.Empty,
                EntrenadorCorreo = row.ContainsKey("EntrenadorCorreo") ? row["EntrenadorCorreo"].ToString() : string.Empty,
                FechaCita = row.ContainsKey("FechaCita") ? Convert.ToDateTime(row["FechaCita"]) : DateTime.MinValue,
                Estado = row.ContainsKey("Estado") ? row["Estado"].ToString() : string.Empty
            };

            return cita;
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
            var cita = (Cita)entity;
            var operation = new SqlOperation { ProcedureName = "CreateCitaMedicionCorporal" };

            operation.AddVarcharParam("CorreoElectronico", cita.CorreoElectronico);
            operation.AddVarcharParam("EntrenadorCorreo", cita.EntrenadorCorreo);
            operation.AddDateTimeParam("FechaCita", cita.FechaCita);
            operation.AddVarcharParam("Estado", cita.Estado);

            return operation;
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "GetCitaByID" };
            operation.AddIntegerParam("CitaID", id);
            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            return new SqlOperation { ProcedureName = "GetAllCitasMedicionCorporal" };
        }

        public SqlOperation GetUpdateStatement(BaseClass entity)
        {
            var cita = (Cita)entity;
            var operation = new SqlOperation { ProcedureName = "UpdateCitaMedicionCorporal" };

            operation.AddVarcharParam("CorreoElectronico", cita.CorreoElectronico);
            operation.AddVarcharParam("EntrenadorCorreo", cita.EntrenadorCorreo);
            operation.AddDateTimeParam("FechaCita", cita.FechaCita);
            operation.AddVarcharParam("Estado", cita.Estado);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseClass entity)
        {
            var cita = (Cita)entity;
            var operation = new SqlOperation { ProcedureName = "DeleteCitaMedicionCorporal" };

            operation.AddVarcharParam("CorreoElectronico", cita.CorreoElectronico);
            operation.AddDateTimeParam("FechaCita", cita.FechaCita);

            return operation;
        }
    }
}
