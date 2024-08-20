using DTO;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using DTO.Rutinas;

namespace BL
{
    public class EjercicioManager
    {
        public void CreateEjercicio(Ejercicio ejercicio)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_RegistrarEjercicio"
            };
            operation.AddVarcharParam("Nombre", ejercicio.Nombre);
            operation.AddVarcharParam("Tipo", ejercicio.Tipo);
            operation.AddVarcharParam("Descripcion", ejercicio.Descripcion);
            operation.AddDecimalParam("Peso", ejercicio.Peso ?? default);
            operation.AddIntegerParam("Tiempo", ejercicio.Tiempo ?? default);
            operation.AddBitParam("AMRAP", ejercicio.Amrap ?? default);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public List<Ejercicio> GetEjercicios()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_ObtenerTodosLosEjercicios"
            };

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            var ejercicios = new List<Ejercicio>();
            foreach (var row in result)
            {
                var ejercicio = new Ejercicio
                {
                    EjercicioId = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    Tipo = row["Tipo"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    Peso = row["Peso"] != DBNull.Value ? (decimal?)row["Peso"] : null,
                    Tiempo = row["Tiempo"] != DBNull.Value ? (int?)row["Tiempo"] : null,
                    Amrap = row["AMRAP"] != DBNull.Value ? (bool?)row["AMRAP"] : null
                };
                ejercicios.Add(ejercicio);
            }

            return ejercicios;
        }

        public Ejercicio GetEjercicioById(int ejercicioId)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_ObtenerEjercicioPorId"
            };
            operation.AddIntegerParam("Id", ejercicioId);

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            if (result.Count > 0)
            {
                var row = result[0];
                var ejercicio = new Ejercicio
                {
                    EjercicioId = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    Tipo = row["Tipo"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    Peso = row["Peso"] != DBNull.Value ? (decimal?)row["Peso"] : null,
                    Tiempo = row["Tiempo"] != DBNull.Value ? (int?)row["Tiempo"] : null,
                    Amrap = row["AMRAP"] != DBNull.Value ? (bool?)row["AMRAP"] : null
                };
                return ejercicio;
            }

            return null;
        }

        public void UpdateEjercicio(Ejercicio ejercicio)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_ActualizarEjercicio"
            };
            operation.AddIntegerParam("Id", ejercicio.EjercicioId);
            operation.AddVarcharParam("Nombre", ejercicio.Nombre);
            operation.AddVarcharParam("Tipo", ejercicio.Tipo);
            operation.AddVarcharParam("Descripcion", ejercicio.Descripcion);
            operation.AddDecimalParam("Peso", ejercicio.Peso ?? default);
            operation.AddIntegerParam("Tiempo", ejercicio.Tiempo ?? default);
            operation.AddBitParam("AMRAP", ejercicio.Amrap ?? default);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public void DeleteEjercicio(int ejercicioId)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_EliminarEjercicio"
            };
            operation.AddIntegerParam("Id", ejercicioId);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }
    }
}
