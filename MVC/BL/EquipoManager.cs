using DTO;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using DTO.Rutinas;

namespace BL
{
    public class EquipoManager
    {
        public void CreateEquipo(Equipo equipo)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_CrearEquipo"
            };
            operation.AddVarcharParam("Nombre", equipo.Nombre);
            operation.AddVarcharParam("Descripcion", equipo.Descripcion);
            operation.AddVarcharParam("GrupoMuscular", equipo.GrupoMuscular);
            operation.AddIntegerParam("Cantidad", equipo.Cantidad);
            operation.AddBitParam("Disponibilidad", equipo.Disponibilidad);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public List<Equipo> GetEquipos()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_ObtenerTodosLosEquipos"
            };

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            var equipos = new List<Equipo>();
            foreach (var row in result)
            {
                var equipo = new Equipo
                {
                    EquipoId = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    GrupoMuscular = row["GrupoMuscular"].ToString(),
                    Cantidad = Convert.ToInt32(row["Cantidad"]),
                    Disponibilidad = Convert.ToBoolean(row["Disponibilidad"])
                };
                equipos.Add(equipo);
            }

            return equipos;
        }

        public Equipo GetEquipoById(int equipoId)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_ObtenerEquipoPorId"
            };
            operation.AddIntegerParam("Id", equipoId);

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            if (result.Count > 0)
            {
                var row = result[0];
                var equipo = new Equipo
                {
                    EquipoId = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    GrupoMuscular = row["GrupoMuscular"].ToString(),
                    Cantidad = Convert.ToInt32(row["Cantidad"]),
                    Disponibilidad = Convert.ToBoolean(row["Disponibilidad"])
                };
                return equipo;
            }

            return null;
        }

        public void UpdateEquipo(Equipo equipo)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_ActualizarEquipo"
            };
            operation.AddIntegerParam("Id", equipo.EquipoId);
            operation.AddVarcharParam("Nombre", equipo.Nombre);
            operation.AddVarcharParam("Descripcion", equipo.Descripcion);
            operation.AddVarcharParam("GrupoMuscular", equipo.GrupoMuscular);
            operation.AddIntegerParam("Cantidad", equipo.Cantidad);
            operation.AddBitParam("Disponibilidad", equipo.Disponibilidad);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public void DeleteEquipo(int equipoId)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_EliminarEquipo"
            };
            operation.AddIntegerParam("Id", equipoId);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }
    }
}
