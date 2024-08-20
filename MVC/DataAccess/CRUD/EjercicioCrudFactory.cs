using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class EjercicioCrudFactory : CrudFactory
    {
        private readonly EjercicioMapper mapper;

        public EjercicioCrudFactory() : base()
        {
            mapper = new EjercicioMapper();
        }

        public override void Create(BaseClass entity)
        {
            var sqlOperation = mapper.GetCreateStatement(entity);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Update(BaseClass entity)
        {
            var sqlOperation = mapper.GetUpdateStatement(entity);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Delete(BaseClass entity)
        {
            var sqlOperation = mapper.GetDeleteStatement(entity);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override T Retrieve<T>(int id)
        {
            var sqlOperation = mapper.GetRetrieveByIdStatement(id);
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                var obj = mapper.BuildObject(lstResult[0]);
                return (T)Convert.ChangeType(obj, typeof(T));
            }

            return default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstEjercicios = new List<T>();

            var sqlOperation = mapper.GetRetrieveAllStatement();
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                var objs = mapper.BuildObjects(lstResult);
                foreach (var obj in objs)
                {
                    lstEjercicios.Add((T)Convert.ChangeType(obj, typeof(T)));
                }
            }

            return lstEjercicios;
        }

        public override BaseClass RetrieveById(int id)
        {
            var sqlOperation = mapper.GetRetrieveByIdStatement(id);
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                return mapper.BuildObject(lstResult[0]);
            }

            return null;
        }
    }
}
