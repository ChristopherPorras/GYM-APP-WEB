using DTO;
using DataAccess.DAO;
using DataAccess.Mapper;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class UserCrudFactory : CrudFactory
    {
        private UserMapper mapper;
        private SqlDao dao;

        public UserCrudFactory()
        {
            mapper = new UserMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseClass entity)
        {
            var user = (User)entity;
            var sqlOperation = mapper.GetCreateStatement(user);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override T Retrieve<T>(int id)
        {
            var sqlOperation = mapper.GetRetrieveByIdStatement(id);
            var result = dao.ExecuteStoredProcedureWithUniqueResult(sqlOperation);
            return result != null ? (T)Convert.ChangeType(mapper.BuildObject(result), typeof(T)) : default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var sqlOperation = mapper.GetRetrieveAllStatement();
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);
            var lstUsers = mapper.BuildObjects(lstResult);
            return (List<T>)(IEnumerable<T>)lstUsers;
        }

        public override void Update(BaseClass entity)
        {
            var user = (User)entity;
            var sqlOperation = mapper.GetUpdateStatement(user);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Delete(BaseClass entity)
        {
            var user = (User)entity;
            var sqlOperation = mapper.GetDeleteStatement(user.CorreoElectronico);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override BaseClass RetrieveById(int id)
        {
            throw new NotImplementedException();
        }

        public User RetrieveByEmail(string correo)
        {
            var sqlOperation = mapper.GetRetrieveByEmailStatement(correo);
            var result = dao.ExecuteStoredProcedureWithUniqueResult(sqlOperation);
            return result != null ? mapper.BuildObject(result) : null;
        }

        public void CreateOTP(OTP otp)
        {
            var sqlOperation = mapper.GetCreateOTPOperation(otp);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public OTP RetrieveOTP(string correo)
        {
            var sqlOperation = mapper.GetRetrieveOTPOperation(correo);
            var result = dao.ExecuteStoredProcedureWithUniqueResult(sqlOperation);
            return result != null ? mapper.BuildOTP(result) : null;
        }

        public void UpdateOTP(OTP otp)
        {
            var sqlOperation = mapper.GetUpdateOTPOperation(otp);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public OTP RetrieveLatestOTP(string correo)
        {
            var sqlOperation = mapper.GetRetrieveLatestOTPOperation(correo);
            var result = dao.ExecuteStoredProcedureWithUniqueResult(sqlOperation);
            return result != null ? mapper.BuildOTP(result) : null;
        }

        public List<User> RetrieveAllEntrenadores()
        {
            var list = new List<User>();
            var sqlOperation = mapper.GetRetrieveAllEntrenadores();
            var results = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (results.Count > 0)
            {
                foreach (var row in results)
                {
                    var obj = mapper.Build1CorreoObject(row);
                    list.Add(obj);
                }
            }

            return list;
        }

        public User RetrieveRolAccess(string correo)
        {
            SqlOperation operation = mapper.RetrieveRolAccess(correo);
            Dictionary<string, object> result = dao.ExecuteStoredProcedureWithUniqueResult(operation);

            if (result != null && result.Count > 0)
            {
                return mapper.Build1RolObject(result);
            }

            return null;
        }
    }
}
    