using DTO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public interface IObjectMapper
    {
        BaseClass BuildObject(Dictionary<string, object> objectRows);
        List<BaseClass> BuildObjects(List<Dictionary<string, object>> objectRows);
    }
}
