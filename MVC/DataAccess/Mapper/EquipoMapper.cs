using DTO;
using DataAccess.DAO;
using System.Collections.Generic;
using DTO.Rutinas;

namespace DataAccess.Mapper
{
    public class EquipoMapper : IObjectMapper, ICrudStatements
    {
        public BaseClass BuildObject(Dictionary<string, object> row)
        {
            var equipo = new Equipo
            {
                EquipoId = (int)row["Id"],
                Nombre = row["Nombre"].ToString(),
                Descripcion = row["Descripcion"].ToString(),
                GrupoMuscular = row["GrupoMuscular"].ToString(),
                Cantidad = (int)row["Cantidad"],
                Disponibilidad = (bool)row["Disponibilidad"]
            };

            return equipo;
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
            var equipo = (Equipo)entity;
            var operation = new SqlOperation { ProcedureName = "sp_CrearEquipo" };

            operation.AddVarcharParam("Nombre", equipo.Nombre);
            operation.AddVarcharParam("Descripcion", equipo.Descripcion);
            operation.AddVarcharParam("GrupoMuscular", equipo.GrupoMuscular);
            operation.AddIntegerParam("Cantidad", equipo.Cantidad);
            operation.AddBitParam("Disponibilidad", equipo.Disponibilidad);

            return operation;
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "sp_ObtenerEquipoPorId" };
            operation.AddIntegerParam("Id", id);
            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            return new SqlOperation { ProcedureName = "sp_ObtenerTodosLosEquipos" };
        }

        public SqlOperation GetUpdateStatement(BaseClass entity)
        {
            var equipo = (Equipo)entity;
            var operation = new SqlOperation { ProcedureName = "sp_ActualizarEquipo" };

            operation.AddIntegerParam("Id", equipo.EquipoId);
            operation.AddVarcharParam("Nombre", equipo.Nombre);
            operation.AddVarcharParam("Descripcion", equipo.Descripcion);
            operation.AddVarcharParam("GrupoMuscular", equipo.GrupoMuscular);
            operation.AddIntegerParam("Cantidad", equipo.Cantidad);
            operation.AddBitParam("Disponibilidad", equipo.Disponibilidad);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseClass entity)
        {
            var equipo = (Equipo)entity;
            var operation = new SqlOperation { ProcedureName = "sp_EliminarEquipo" };

            operation.AddIntegerParam("Id", equipo.EquipoId);

            return operation;
        }
    }
}
