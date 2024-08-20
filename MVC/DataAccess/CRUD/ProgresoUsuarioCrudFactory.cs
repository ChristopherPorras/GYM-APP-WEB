using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class ProgresoUsuarioCrudFactory : CrudFactory
    {
        private readonly ProgresoUsuarioMapper mapper;
        private readonly SqlDao dao;

        public ProgresoUsuarioCrudFactory()
        {
            mapper = new ProgresoUsuarioMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseClass entity)
        {
            var progreso = (ProgresoUsuarioDTO)entity;
            var sqlOperation = mapper.GetCreateStatement(progreso);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Delete(BaseClass entity)
        {
            var progreso = (ProgresoUsuarioDTO)entity;
            var sqlOperation = mapper.GetDeleteStatement(progreso.ID);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override T Retrieve<T>(int id)
        {
            var sqlOperation = mapper.GetRetrieveByIdStatement(id);
            var result = dao.ExecuteStoredProcedureWithUniqueResult(sqlOperation);
            return result != null ? (T)Convert.ChangeType(mapper.BuildObject(result), typeof(T)) : default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var sqlOperation = mapper.GetRetrieveAllStatement();
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);
            var lstProgreso = mapper.BuildObjects(lstResult);
            return (List<T>)(IEnumerable<T>)lstProgreso;
        }

        public override void Update(BaseClass entity)
        {
            var progreso = (ProgresoUsuarioDTO)entity;
            var sqlOperation = mapper.GetUpdateStatement(progreso);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public List<ProgresoUsuarioDTO> RetrieveByCorreo(string correo)
        {
            var sqlOperation = mapper.GetRetrieveByCorreoStatement(correo);
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);
            return mapper.BuildObjects(lstResult);
        }

        public override BaseClass RetrieveById(int id)
        {
            var sqlOperation = mapper.GetRetrieveByIdStatement(id);
            var result = dao.ExecuteStoredProcedureWithUniqueResult(sqlOperation);
            return result != null ? mapper.BuildObject(result) : null;
        }
    }
}
