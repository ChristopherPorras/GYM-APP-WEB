using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace BL
{
    public class ClaseGrupalManager
    {
        private SqlDao dao;

        public ClaseGrupalManager()
        {
            dao = SqlDao.GetInstance();
        }

        public async Task CreateClaseGrupal(ClaseGrupal claseGrupal)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "CreateClaseGrupal"
            };
            operation.AddVarcharParam("Nombre", claseGrupal.Nombre);
            operation.AddVarcharParam("Descripcion", claseGrupal.Descripcion);
            operation.AddIntegerParam("Cupo", claseGrupal.Cupo);
            operation.AddDateTimeParam("Horario", claseGrupal.Horario);
            operation.AddVarcharParam("UsuarioCorreo", claseGrupal.UsuarioCorreo);
            operation.AddVarcharParam("EntrenadorCorreo", claseGrupal.EntrenadorCorreo);

            await dao.ExecuteStoredProcedureWithQueryAsync(operation);
        }

        public async Task<List<ClaseGrupal>> GetClasesGrupales()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetAllClasesGrupales"
            };

            var result = await dao.ExecuteStoredProcedureWithQueryAsync(operation);

            var clasesGrupales = new List<ClaseGrupal>();
            foreach (var row in result)
            {
                var claseGrupal = new ClaseGrupal
                {
                    ClaseGrupalID = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    Cupo = Convert.ToInt32(row["Cupo"]),
                    Horario = Convert.ToDateTime(row["Horario"]),
                    UsuarioCorreo = row["UsuarioCorreo"].ToString(),
                    EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
                };
                clasesGrupales.Add(claseGrupal);
            }

            return clasesGrupales;
        }

        public async Task<ClaseGrupal> GetClaseGrupalById(int id)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetClaseGrupalByID"
            };
            operation.AddIntegerParam("Id", id);

            var result = await dao.ExecuteStoredProcedureWithQueryAsync(operation);

            if (result.Count > 0)
            {
                var row = result[0];
                return new ClaseGrupal
                {
                    ClaseGrupalID = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    Cupo = Convert.ToInt32(row["Cupo"]),
                    Horario = Convert.ToDateTime(row["Horario"]),
                    UsuarioCorreo = row["UsuarioCorreo"].ToString(),
                    EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
                };
            }
            return null;
        }

        public async Task<List<ClaseGrupal>> ClasesGrupalesPorNombre(string nombreClase)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "ObtenerClasesGrupalesPorNombre"
            };
            operation.AddVarcharParam("Nombre", nombreClase);

            var result = await dao.ExecuteStoredProcedureWithQueryAsync(operation);

            var clasesGrupales = new List<ClaseGrupal>();
            foreach (var row in result)
            {
                var claseGrupal = new ClaseGrupal
                {
                    ClaseGrupalID = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    Cupo = Convert.ToInt32(row["Cupo"]),
                    Horario = Convert.ToDateTime(row["Horario"]),
                    UsuarioCorreo = row["UsuarioCorreo"].ToString(),
                    EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
                };
                clasesGrupales.Add(claseGrupal);
            }

            return clasesGrupales;
        }

        public async Task UpdateClaseGrupal(ClaseGrupal claseGrupal)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "UpdateClaseGrupal"
            };
            operation.AddIntegerParam("Id", claseGrupal.ClaseGrupalID);
            operation.AddVarcharParam("Nombre", claseGrupal.Nombre);
            operation.AddVarcharParam("Descripcion", claseGrupal.Descripcion);
            operation.AddIntegerParam("Cupo", claseGrupal.Cupo);
            operation.AddDateTimeParam("Horario", claseGrupal.Horario);
            operation.AddVarcharParam("UsuarioCorreo", claseGrupal.UsuarioCorreo);
            operation.AddVarcharParam("EntrenadorCorreo", claseGrupal.EntrenadorCorreo);

            await dao.ExecuteStoredProcedureWithQueryAsync(operation);
        }

        public async Task DeleteClaseGrupal(int id)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "DeleteClaseGrupal"
            };
            operation.AddIntegerParam("Id", id);

            await dao.ExecuteStoredProcedureWithQueryAsync(operation);
        }
    }
}
