using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;

namespace DataAccess.CRUD
{
    public class CuponesCrudFactory : CrudFactory
    {
        private readonly CuponesMapper _mapper;

        public CuponesCrudFactory()
        {
            _mapper = new CuponesMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseClass entity)
        {
            var operation = _mapper.GetCreateStatement(entity);
            dao.ExecuteStoredProcedure(operation);
        }

        public override void Update(BaseClass entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(BaseClass entity)
        {
            throw new NotImplementedException();
        }

        public override T Retrieve<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            throw new NotImplementedException();
        }

        public void ApplyCoupon(string codigo, string correoElectronico)
        {
            var operation = _mapper.GetApplyCouponStatement(codigo, correoElectronico);
            dao.ExecuteStoredProcedure(operation);
        }

        public override BaseClass RetrieveById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
