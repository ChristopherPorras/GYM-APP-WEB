using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class RutinaCrudFactory : CrudFactory
    {
        private readonly SqlDao dao;

        public RutinaCrudFactory()
        {
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseClass entity)
        {
            var rutina = (RutinaDTO)entity;
            var operation = new SqlOperation
            {
                ProcedureName = "sp_CreateRutina"
            };
            operation.AddVarcharParam("CorreoElectronico", rutina.CorreoElectronico);
            operation.AddIntegerParam("MedicionId", rutina.MedicionId);
            operation.AddVarcharParam("EntrenadorCorreo", rutina.EntrenadorCorreo);

            dao.ExecuteStoredProcedure(operation);
        }

        public override T Retrieve<T>(int id)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_GetRutinaById"
            };
            operation.AddIntegerParam("ID", id);
            var result = dao.ExecuteStoredProcedureWithUniqueResult(operation);
            if (result != null)
            {
                var mapper = new RutinaMapper();
                return (T)Convert.ChangeType(mapper.BuildObject(result), typeof(T));
            }
            return default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_GetAllRutinas"
            };
            var results = dao.ExecuteStoredProcedureWithQuery(operation);
            var mapper = new RutinaMapper();
            var list = new List<T>();

            foreach (var row in results)
            {
                var rutina = (T)Convert.ChangeType(mapper.BuildObject(row), typeof(T));
                list.Add(rutina);
            }

            return list;
        }

        public override void Update(BaseClass entity)
        {
            var rutina = (RutinaDTO)entity;
            var operation = new SqlOperation
            {
                ProcedureName = "sp_UpdateRutina"
            };
            operation.AddIntegerParam("ID", rutina.ID);
            operation.AddVarcharParam("CorreoElectronico", rutina.CorreoElectronico);
            operation.AddIntegerParam("MedicionId", rutina.MedicionId);
            operation.AddVarcharParam("EntrenadorCorreo", rutina.EntrenadorCorreo);

            dao.ExecuteStoredProcedure(operation);
        }

        public override void Delete(BaseClass entity)
        {
            var rutina = (RutinaDTO)entity;
            var operation = new SqlOperation
            {
                ProcedureName = "sp_DeleteRutina"
            };
            operation.AddIntegerParam("ID", rutina.ID);

            dao.ExecuteStoredProcedure(operation);
        }

        // Método para obtener rutinas por correo
        public List<T> RetrieveByCorreo<T>(string correo)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_GetRutinasByCorreo"
            };
            operation.AddVarcharParam("CorreoElectronico", correo);
            var results = dao.ExecuteStoredProcedureWithQuery(operation);
            var mapper = new RutinaMapper();
            var list = new List<T>();

            foreach (var row in results)
            {
                var rutina = (T)Convert.ChangeType(mapper.BuildObject(row), typeof(T));
                list.Add(rutina);
            }

            return list;
        }

        public override BaseClass RetrieveById(int id)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "sp_GetRutinaById"
            };
            operation.AddIntegerParam("ID", id);
            var result = dao.ExecuteStoredProcedureWithUniqueResult(operation);
            if (result != null)
            {
                var mapper = new RutinaMapper();
                return mapper.BuildObject(result);
            }
            return null;
        }

    }
}
