using DataAccess.DAO;
using DTO;
using System;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class UserMapper
    {
        public User Build1RolObject(Dictionary<string, object> result)
        {
            var user = new User
            {
                RolAcceso = result["Rol"].ToString()
            };

            return user;
        }

        public User Build1CorreoObject(Dictionary<string, object> result)
        {
            var user = new User
            {
                CorreoElectronico = result["CorreoElectronico"].ToString()
            };
            return user;
        }

        public SqlOperation GetCreateStatement(User user)
        {
            var operation = new SqlOperation { ProcedureName = "RegistrarUsuario" };

            operation.AddVarcharParam("CorreoElectronico", user.CorreoElectronico);
            operation.AddVarcharParam("Nombre", user.Nombre);
            operation.AddVarcharParam("Contrasena", user.Contrasena);
            operation.AddVarcharParam("Telefono", user.Telefono);
            operation.AddVarcharParam("TipoUsuario", user.TipoUsuario);
            operation.AddDateTimeParam("FechaRegistro", user.FechaRegistro);
            operation.AddBitParam("CorreoVerificado", user.CorreoVerificado);
            operation.AddBitParam("TelefonoVerificado", user.TelefonoVerificado);
            operation.AddBitParam("Estado", user.Estado);
            operation.AddBitParam("HaPagado", user.HaPagado);

            return operation;
        }

        public SqlOperation GetUpdateOTPOperation(OTP otp)
        {
            var operation = new SqlOperation { ProcedureName = "UpdateOTP" };
            operation.AddIntegerParam("Id", otp.ID);
            operation.AddVarcharParam("CodigoOTP", otp.CodigoOTP);
            operation.AddBitParam("Usado", otp.Usado);
            operation.AddDateTimeParam("FechaCreacion", otp.FechaCreacion);
            return operation;
        }

        public SqlOperation GetUpdateStatement(User user)
        {
            var operation = new SqlOperation { ProcedureName = "sp_ActualizarUsuario" };

            operation.AddVarcharParam("CorreoElectronico", user.CorreoElectronico);
            operation.AddVarcharParam("Nombre", user.Nombre);
            operation.AddVarcharParam("Contrasena", user.Contrasena);
            operation.AddVarcharParam("Telefono", user.Telefono);
            operation.AddVarcharParam("TipoUsuario", user.TipoUsuario);
            operation.AddDateTimeParam("FechaRegistro", user.FechaRegistro);
            operation.AddBitParam("CorreoVerificado", user.CorreoVerificado);
            operation.AddBitParam("TelefonoVerificado", user.TelefonoVerificado);
            operation.AddBitParam("Estado", user.Estado);
            operation.AddBitParam("HaPagado", user.HaPagado);

            return operation;
        }

        public SqlOperation GetRetrieveStatement(string correo)
        {
            var operation = new SqlOperation { ProcedureName = "sp_ObtenerUsuario" };
            operation.AddVarcharParam("CorreoElectronico", correo);
            return operation;
        }

        public SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "sp_ObtenerUsuarioPorId" };

            operation.AddIntegerParam("Id", id);
            return operation;
        }

        public SqlOperation GetRetrieveByEmailStatement(string correo)
        {
            var operation = new SqlOperation { ProcedureName = "sp_ObtenerUsuarioPorCorreo" };
            operation.AddVarcharParam("CorreoElectronico", correo);
            return operation;
        }

        public SqlOperation GetDeleteStatement(string correo)
        {
            var operation = new SqlOperation { ProcedureName = "sp_EliminarUsuario" };

            operation.AddVarcharParam("CorreoElectronico", correo);
            return operation;
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            return new SqlOperation { ProcedureName = "sp_ObtenerUsuarios" };
        }

        public SqlOperation GetRetrieveOTPOperation(string email)
        {
            var operation = new SqlOperation { ProcedureName = "RetrieveOTP" };

            operation.AddVarcharParam("CorreoElectronico", email);
            return operation;
        }

        public SqlOperation GetCreateOTPOperation(OTP otp)
        {
            var operation = new SqlOperation { ProcedureName = "CreateOTP" };

            operation.AddVarcharParam("CorreoElectronico", otp.CorreoElectronico);
            operation.AddVarcharParam("CodigoOTP", otp.CodigoOTP);
            operation.AddDateTimeParam("FechaCreacion", otp.FechaCreacion);
            operation.AddBitParam("Usado", otp.Usado);
            return operation;
        }

        public OTP BuildOTP(Dictionary<string, object> row)
        {
            return new OTP
            {
                ID = Convert.ToInt32(row["ID"]), // Ensure this matches the ID column from the SQL query
                CorreoElectronico = row["CorreoElectronico"].ToString(),
                CodigoOTP = row["CodigoOTP"].ToString(),
                FechaCreacion = Convert.ToDateTime(row["FechaCreacion"]),
                Usado = Convert.ToBoolean(row["Usado"])
            };
        }


        public User BuildObject(Dictionary<string, object> row)
        {
            return new User
            {
                CorreoElectronico = row["CorreoElectronico"].ToString(),
                Nombre = row["Nombre"].ToString(),
                Contrasena = row["Contrasena"].ToString(),
                Telefono = row["Telefono"].ToString(),
                TipoUsuario = row["TipoUsuario"].ToString(),
                FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                CorreoVerificado = Convert.ToBoolean(row["CorreoVerificado"]),
                TelefonoVerificado = Convert.ToBoolean(row["TelefonoVerificado"]),
                Estado = Convert.ToBoolean(row["Estado"]),
                HaPagado = Convert.ToBoolean(row["HaPagado"])
            };
        }

        public List<User> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<User>();

            foreach (var row in lstRows)
            {
                var user = BuildObject(row);
                lstResults.Add(user);
            }

            return lstResults;
        }

        public SqlOperation GetRetrieveLatestOTPOperation(string email)
        {
            var operation = new SqlOperation { ProcedureName = "RetrieveLatestOTP" };
            operation.AddVarcharParam("CorreoElectronico", email);
            return operation;
        }

        public SqlOperation GetRetrieveAllEntrenadores()
        {
            var operation = new SqlOperation { ProcedureName = "ObtenerUsuariosPorRol" };
            operation.AddIntegerParam("RolId", 2);
            return operation;
        }

        public SqlOperation RetrieveRolAccess(string correo)
        {
            var operation = new SqlOperation { ProcedureName = "DevolverRolAcceso" };
            operation.AddVarcharParam("CorreoElectronico", correo);
            return operation;
        }
    }
}
