using DTO;
using DataAccess.DAO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class ClaseGrupalMapper : IObjectMapper, ICrudStatements
    {
        public BaseClass BuildObject(Dictionary<string, object> row)
        {
            var claseGrupal = new ClaseGrupal
            {
                ClaseGrupalID = (int)row["Id"],
                Nombre = row["Nombre"].ToString(),
                Descripcion = row["Descripcion"].ToString(),
                Cupo = (int)row["Cupo"],
                Horario = (DateTime)row["Horario"],
                UsuarioCorreo = row["UsuarioCorreo"].ToString(),
                EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
            };

            return claseGrupal;
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
            var claseGrupal = (ClaseGrupal)entity;
            var operation = new SqlOperation { ProcedureName = "CreateClaseGrupal" };

            operation.AddVarcharParam("Nombre", claseGrupal.Nombre);
            operation.AddVarcharParam("Descripcion", claseGrupal.Descripcion);
            operation.AddIntegerParam("Cupo", claseGrupal.Cupo);
            operation.AddDateTimeParam("Horario", claseGrupal.Horario);
            operation.AddVarcharParam("UsuarioCorreo", claseGrupal.UsuarioCorreo);
            operation.AddVarcharParam("EntrenadorCorreo", claseGrupal.EntrenadorCorreo);

            return operation;
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "GetClaseGrupalByID" };
            operation.AddIntegerParam("Id", id);
            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            return new SqlOperation { ProcedureName = "GetAllClasesGrupales" };
        }

        public SqlOperation GetUpdateStatement(BaseClass entity)
        {
            var claseGrupal = (ClaseGrupal)entity;
            var operation = new SqlOperation { ProcedureName = "UpdateClaseGrupal" };

            operation.AddIntegerParam("Id", claseGrupal.ClaseGrupalID);
            operation.AddVarcharParam("Nombre", claseGrupal.Nombre);
            operation.AddVarcharParam("Descripcion", claseGrupal.Descripcion);
            operation.AddIntegerParam("Cupo", claseGrupal.Cupo);
            operation.AddDateTimeParam("Horario", claseGrupal.Horario);
            operation.AddVarcharParam("UsuarioCorreo", claseGrupal.UsuarioCorreo);
            operation.AddVarcharParam("EntrenadorCorreo", claseGrupal.EntrenadorCorreo);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseClass entity)
        {
            var claseGrupal = (ClaseGrupal)entity;
            var operation = new SqlOperation { ProcedureName = "DeleteClaseGrupal" };

            operation.AddIntegerParam("Id", claseGrupal.ClaseGrupalID);

            return operation;
        }

        // Nuevo método para obtener clases grupales por nombre
        public SqlOperation GetRetrieveByNombreStatement(string nombre)
        {
            var operation = new SqlOperation { ProcedureName = "ObtenerClasesGrupalesPorNombre" };
            operation.AddVarcharParam("Nombre", nombre);
            return operation;
        }
    }
}
