using DTO;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using DTO.Rutinas;

namespace DataAccess.Mapper
{
    public class EjercicioMapper : IObjectMapper, ICrudStatements
    {
        public BaseClass BuildObject(Dictionary<string, object> row)
        {
            var ejercicio = new Ejercicio
            {
                EjercicioId = (int)row["Id"],
                Nombre = row["Nombre"].ToString(),
                Tipo = row["Tipo"].ToString(),
                Descripcion = row["Descripcion"].ToString(),
                Peso = row["Peso"] != DBNull.Value ? (decimal?)row["Peso"] : null,
                Tiempo = row["Tiempo"] != DBNull.Value ? (int?)row["Tiempo"] : null,
                Amrap = row["AMRAP"] != DBNull.Value ? (bool?)row["AMRAP"] : null
            };

            return ejercicio;
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
            var ejercicio = (Ejercicio)entity;
            var operation = new SqlOperation { ProcedureName = "sp_RegistrarEjercicio" };

            operation.AddVarcharParam("Nombre", ejercicio.Nombre);
            operation.AddVarcharParam("Tipo", ejercicio.Tipo);
            operation.AddVarcharParam("Descripcion", ejercicio.Descripcion);
            operation.AddDecimalParam("Peso", ejercicio.Peso ?? default);
            operation.AddIntegerParam("Tiempo", ejercicio.Tiempo ?? default);
            operation.AddBitParam("AMRAP", ejercicio.Amrap ?? default);

            return operation;
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "sp_ObtenerEjercicioPorId" };
            operation.AddIntegerParam("Id", id);
            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            return new SqlOperation { ProcedureName = "sp_ObtenerTodosLosEjercicios" };
        }

        public SqlOperation GetUpdateStatement(BaseClass entity)
        {
            var ejercicio = (Ejercicio)entity;
            var operation = new SqlOperation { ProcedureName = "sp_ActualizarEjercicio" };

            operation.AddIntegerParam("Id", ejercicio.EjercicioId);
            operation.AddVarcharParam("Nombre", ejercicio.Nombre);
            operation.AddVarcharParam("Tipo", ejercicio.Tipo);
            operation.AddVarcharParam("Descripcion", ejercicio.Descripcion);
            operation.AddDecimalParam("Peso", ejercicio.Peso ?? default);
            operation.AddIntegerParam("Tiempo", ejercicio.Tiempo ?? default);
            operation.AddBitParam("AMRAP", ejercicio.Amrap ?? default);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseClass entity)
        {
            var ejercicio = (Ejercicio)entity;
            var operation = new SqlOperation { ProcedureName = "sp_EliminarEjercicio" };

            operation.AddIntegerParam("Id", ejercicio.EjercicioId);

            return operation;
        }
    }
}
