using System.Collections.Generic;
using System.Data;

namespace DataAccess.Mapper
{
    public abstract class MapperBase<T>
    {
        public abstract T BuildObject(IDataReader reader);
        public abstract T BuildObject(Dictionary<string, object> row);
        public abstract List<T> BuildObjects(List<Dictionary<string, object>> lstRows);
    }
}
