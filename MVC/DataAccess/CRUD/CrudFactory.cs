using DataAccess.DAO;
using DTO;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public abstract class CrudFactory
    {
        protected SqlDao dao;

        public abstract void Create(BaseClass entity);
        public abstract T Retrieve<T>(int id);
        public abstract List<T> RetrieveAll<T>();
        public abstract void Update(BaseClass entity);
        public abstract void Delete(BaseClass entity);
        public abstract BaseClass RetrieveById(int id);
    }
}
