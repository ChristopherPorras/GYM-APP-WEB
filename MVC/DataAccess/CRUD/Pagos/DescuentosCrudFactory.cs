using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class DescuentosCrudFactory : CrudFactory
    {
        private readonly DescuentosMapper mapper;
        private readonly SqlDao dao;

        public DescuentosCrudFactory()
        {
            mapper = new DescuentosMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseClass entity)
        {
            var descuento = (Descuentos)entity;
            var sqlOperation = mapper.GetCreateStatement(descuento);
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
            var lstDescuentos = new List<T>();

            var sqlOperation = mapper.GetRetrieveAllStatement();
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                var objs = mapper.BuildObjects(lstResult);
                foreach (var obj in objs)
                {
                    lstDescuentos.Add((T)Convert.ChangeType(obj, typeof(T)));
                }
            }

            return lstDescuentos;
        }

        public override void Update(BaseClass entity)
        {
            var descuento = (Descuentos)entity;
            var sqlOperation = mapper.GetUpdateStatement(descuento);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Delete(BaseClass entity)
        {
            var descuento = (Descuentos)entity;
            var sqlOperation = mapper.GetDeleteStatement(descuento);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override BaseClass RetrieveById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
