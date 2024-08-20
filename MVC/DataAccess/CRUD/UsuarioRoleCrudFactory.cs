using DataAccess.DAO;
using DTO;
using DataAccess.Mapper;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.CRUD
{
    public class UsuarioRoleCrudFactory : CrudFactory
    {
        private UsuarioRoleMapper mapper;

        public UsuarioRoleCrudFactory()
        {
            mapper = new UsuarioRoleMapper();
            dao = SqlDao.GetInstance();
        }

        public void CreateUsuarioConRol(UsuarioRole usuarioRole)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(usuarioRole.Contrasena, salt);
            usuarioRole.Contrasena = hashedPassword;
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CreateUsuarioConRol"
            };
            sqlOperation.AddVarcharParam("CorreoElectronico", usuarioRole.CorreoElectronico);
            sqlOperation.AddNVarcharParam("Nombre", usuarioRole.Nombre);
            sqlOperation.AddNVarcharParam("Contrasena", usuarioRole.Contrasena);
            sqlOperation.AddDateTimeParam("FechaRegistro", usuarioRole.FechaRegistro);
            sqlOperation.AddNVarcharParam("Telefono", usuarioRole.Telefono);
            sqlOperation.AddNVarcharParam("TipoUsuario", usuarioRole.TipoUsuario);
            sqlOperation.AddBitParam("Estado", usuarioRole.Estado);
            sqlOperation.AddBitParam("HaPagado", usuarioRole.HaPagado);
            sqlOperation.AddBitParam("CorreoVerificado", usuarioRole.CorreoVerificado);
            sqlOperation.AddBitParam("TelefonoVerificado", usuarioRole.TelefonoVerificado);
            sqlOperation.AddIntegerParam("RolId", usuarioRole.RolId);

            dao.ExecuteStoredProcedure(sqlOperation);

            DeleteSaltIfExists(usuarioRole.CorreoElectronico);

            InsertSalt(usuarioRole.CorreoElectronico, salt);
        }

        private void DeleteSaltIfExists(string correoElectronico)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DeleteSaltIfExists"
            };
            sqlOperation.AddVarcharParam("CorreoElectronico", correoElectronico);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Create(BaseClass entity)
        {
            var usuarioRole = (UsuarioRole)entity;
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(usuarioRole.Contrasena, salt);
            usuarioRole.Contrasena = hashedPassword;
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CreateUsuarioConRol"
            };
            sqlOperation.AddVarcharParam("CorreoElectronico", usuarioRole.CorreoElectronico);
            sqlOperation.AddNVarcharParam("Nombre", usuarioRole.Nombre);
            sqlOperation.AddNVarcharParam("Contrasena", usuarioRole.Contrasena);
            sqlOperation.AddDateTimeParam("FechaRegistro", usuarioRole.FechaRegistro);
            sqlOperation.AddNVarcharParam("Telefono", usuarioRole.Telefono);
            sqlOperation.AddNVarcharParam("TipoUsuario", usuarioRole.TipoUsuario);
            sqlOperation.AddBitParam("Estado", usuarioRole.Estado);
            sqlOperation.AddBitParam("HaPagado", usuarioRole.HaPagado);
            sqlOperation.AddBitParam("CorreoVerificado", usuarioRole.CorreoVerificado);
            sqlOperation.AddBitParam("TelefonoVerificado", usuarioRole.TelefonoVerificado);
            sqlOperation.AddIntegerParam("RolId", usuarioRole.RolId);

            dao.ExecuteStoredProcedure(sqlOperation);
            DeleteSaltIfExists(usuarioRole.CorreoElectronico);
            InsertSalt(usuarioRole.CorreoElectronico, salt);
        }



        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        private void InsertSalt(string correoElectronico, string salt)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "InsertSalt"
            };

            sqlOperation.AddVarcharParam("CorreoElectronico", correoElectronico);
            sqlOperation.AddNVarcharParam("Salt", salt);

            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public UsuarioRole RetrieveByEmail(string correoElectronico)
        {
            var sqlOperation = mapper.GetRetrieveByEmailStatement(correoElectronico);
            var result = dao.ExecuteStoredProcedureWithQuery(sqlOperation);
            if (result.Count > 0)
            {
                var obj = mapper.BuildObject(result[0]);
                return (UsuarioRole)Convert.ChangeType(obj, typeof(UsuarioRole));
            }

            return null;
        }

        public override T Retrieve<T>(int id)
        {
            var sqlOperation = mapper.GetRetrieveByIdStatement(id);
            var result = dao.ExecuteStoredProcedureWithQuery(sqlOperation);
            if (result.Count > 0)
            {
                var obj = mapper.BuildObject(result[0]);
                return (T)Convert.ChangeType(obj, typeof(T));
            }

            return default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var list = new List<T>();
            var sqlOperation = mapper.GetRetrieveAllStatement();
            var results = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (results.Count > 0)
            {
                foreach (var row in results)
                {
                    var obj = mapper.BuildObject(row);
                    list.Add((T)Convert.ChangeType(obj, typeof(T)));
                }
            }

            return list;
        }

        public override void Update(BaseClass entity)
        {
            if (entity is UsuarioRoleUpdate usuarioRoleUpdate)
            {
                var sqlOperation = mapper.GetUpdateStatement(usuarioRoleUpdate);
                dao.ExecuteStoredProcedure(sqlOperation);
            }
            else
            {
                throw new InvalidCastException("Entity is not of type UsuarioRoleUpdate");
            }
        }

        public override void Delete(BaseClass entity)
        {
            var usuarioRole = (UsuarioRole)entity;
            var sqlOperation = mapper.GetDeleteStatement(usuarioRole);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override BaseClass RetrieveById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
