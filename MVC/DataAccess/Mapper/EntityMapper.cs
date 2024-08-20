using DataAccess.DAO;
using DTO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public abstract class EntityMapper
    {
        public abstract SqlOperation GetCreateStatement(BaseClass entity);
        public abstract SqlOperation GetRetrieveByIdStatement(int id);
        public abstract SqlOperation GetRetrieveAllStatement();
        public abstract SqlOperation GetUpdateStatement(BaseClass entity);
        public abstract SqlOperation GetDeleteStatement(BaseClass entity);
        public abstract BaseClass BuildObject(Dictionary<string, object> row);
        public abstract List<BaseClass> BuildObjects(List<Dictionary<string, object>> lstRows);
    }
}
