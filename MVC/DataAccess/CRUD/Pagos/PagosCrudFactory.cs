using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class PagosCrudFactory : CrudFactory
    {
        private readonly PagosMapper mapper;
        private readonly SqlDao dao;

        public PagosCrudFactory()
        {
            mapper = new PagosMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseClass entity)
        {
            var pago = (Pagos)entity;
            var sqlOperation = mapper.GetCreateStatement(pago);
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
            var lstPagos = new List<T>();

            var sqlOperation = mapper.GetRetrieveAllStatement();
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                var objs = mapper.BuildObjects(lstResult);
                foreach (var obj in objs)
                {
                    lstPagos.Add((T)Convert.ChangeType(obj, typeof(T)));
                }
            }

            return lstPagos;
        }

        public override void Update(BaseClass entity)
        {
            var pago = (Pagos)entity;
            var sqlOperation = mapper.GetUpdateStatement(pago);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Delete(BaseClass entity)
        {
            var pago = (Pagos)entity;
            var sqlOperation = mapper.GetDeleteStatement(pago);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override BaseClass RetrieveById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
