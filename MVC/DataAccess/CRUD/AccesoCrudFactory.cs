using DTO;
using DataAccess.DAO;
using DataAccess.Mapper;
using System;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class AccesoCrudFactory : CrudFactory
    {
        private AccesoMapper mapper;

        public AccesoCrudFactory()
        {
            mapper = new AccesoMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseClass entity)
        {
            var acceso = (Acceso)entity;
            var sqlOperation = mapper.GetCreateStatement(acceso);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override T Retrieve<T>(int id)
        {
            var sqlOperation = mapper.GetRetrieveByIdStatement(id);
            var result = dao.ExecuteStoredProcedureWithQuery(sqlOperation);
            if (result.Count > 0)
            {
                var obj = mapper.BuildObject(result[0]);
                return (T)Convert.ChangeType(obj, typeof(T));
            }

            return default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var list = new List<T>();
            var sqlOperation = mapper.GetRetrieveAllStatement();
            var results = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (results.Count > 0)
            {
                foreach (var row in results)
                {
                    var obj = mapper.BuildObject(row);
                    list.Add((T)Convert.ChangeType(obj, typeof(T)));
                }
            }

            return list;
        }

        public override void Update(BaseClass entity)
        {
            var acceso = (Acceso)entity;
            var sqlOperation = mapper.GetUpdateStatement(acceso);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Delete(BaseClass entity)
        {
            var acceso = (Acceso)entity;
            var sqlOperation = mapper.GetDeleteStatement(acceso);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override BaseClass RetrieveById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
