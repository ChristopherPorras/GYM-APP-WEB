using DataAccess.DAO;
using DTO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class ProgresoUsuarioMapper
    {
        public SqlOperation GetCreateStatement(ProgresoUsuarioDTO progreso)
        {
            var operation = new SqlOperation { ProcedureName = "sp_CreateProgresoUsuario" };
            operation.AddVarcharParam("CorreoElectronico", progreso.CorreoElectronico);
            operation.AddDateTimeParam("FechaProgreso", progreso.FechaProgreso);
            operation.AddDecimalParam("Peso", progreso.Peso);
            operation.AddDecimalParam("MasaMuscular", progreso.MasaMuscular);
            operation.AddDecimalParam("PorcentajeGrasa", progreso.PorcentajeGrasa);
            return operation;
        }

        public SqlOperation GetDeleteStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "sp_DeleteProgresoUsuario" };
            operation.AddIntegerParam("Id", id);
            return operation;
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "sp_GetProgresoUsuarioById" };
            operation.AddIntegerParam("Id", id);
            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            return new SqlOperation { ProcedureName = "sp_GetAllProgresoUsuario" };
        }

        public SqlOperation GetUpdateStatement(ProgresoUsuarioDTO progreso)
        {
            var operation = new SqlOperation { ProcedureName = "sp_UpdateProgresoUsuario" };
            operation.AddIntegerParam("Id", progreso.ID);
            operation.AddVarcharParam("CorreoElectronico", progreso.CorreoElectronico);
            operation.AddDateTimeParam("FechaProgreso", progreso.FechaProgreso);
            operation.AddDecimalParam("Peso", progreso.Peso);
            operation.AddDecimalParam("MasaMuscular", progreso.MasaMuscular);
            operation.AddDecimalParam("PorcentajeGrasa", progreso.PorcentajeGrasa);
            return operation;
        }

        public SqlOperation GetRetrieveByCorreoStatement(string correo)
        {
            var operation = new SqlOperation { ProcedureName = "sp_GetProgresoUsuarioByCorreo" };
            operation.AddVarcharParam("CorreoElectronico", correo);
            return operation;
        }

        public ProgresoUsuarioDTO BuildObject(Dictionary<string, object> row)
        {
            return new ProgresoUsuarioDTO
            {
                ID = (int)row["Id"],
                CorreoElectronico = row["CorreoElectronico"].ToString(),
                FechaProgreso = (DateTime)row["FechaProgreso"],
                Peso = (decimal)row["Peso"],
                MasaMuscular = (decimal)row["MasaMuscular"],
                PorcentajeGrasa = (decimal)row["PorcentajeGrasa"]
            };
        }

        public List<ProgresoUsuarioDTO> BuildObjects(List<Dictionary<string, object>> rows)
        {
            var results = new List<ProgresoUsuarioDTO>();

            foreach (var row in rows)
            {
                results.Add(BuildObject(row));
            }

            return results;
        }
    }
}
