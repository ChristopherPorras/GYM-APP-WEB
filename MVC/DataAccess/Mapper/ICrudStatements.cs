using DataAccess.DAO;
using DTO;

namespace DataAccess.Mapper
{
    internal interface ICrudStatements
    {
        SqlOperation GetCreateStatement(BaseClass entityDTO);
        SqlOperation GetUpdateStatement(BaseClass entityDTO);
        SqlOperation GetDeleteStatement(BaseClass entityDTO);
        SqlOperation GetRetrieveAllStatement();
        SqlOperation GetRetrieveByIdStatement(int Id);
    }
}
