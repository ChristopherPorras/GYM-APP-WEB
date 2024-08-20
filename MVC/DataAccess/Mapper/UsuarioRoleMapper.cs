using DataAccess.DAO;
using DTO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class UsuarioRoleMapper : EntityMapper
    {
        public override SqlOperation GetCreateStatement(BaseClass entity)
        {
            var usuarioRole = (UsuarioRole)entity;
            var operation = new SqlOperation
            {
                ProcedureName = "CreateUsuarioConRol"
            };
            operation.AddVarcharParam("CorreoElectronico", usuarioRole.CorreoElectronico);
            operation.AddVarcharParam("Nombre", usuarioRole.Nombre);
            operation.AddVarcharParam("Contrasena", usuarioRole.Contrasena);
            operation.AddDateTimeParam("FechaRegistro", usuarioRole.FechaRegistro);
            operation.AddVarcharParam("Telefono", usuarioRole.Telefono);
            operation.AddVarcharParam("TipoUsuario", usuarioRole.TipoUsuario);
            operation.AddBitParam("Estado", usuarioRole.Estado);
            operation.AddBitParam("HaPagado", usuarioRole.HaPagado);
            operation.AddBitParam("CorreoVerificado", usuarioRole.CorreoVerificado);
            operation.AddBitParam("TelefonoVerificado", usuarioRole.TelefonoVerificado);
            operation.AddIntegerParam("RolId", usuarioRole.RolId);
            return operation;
        }

        public override SqlOperation GetUpdateStatement(BaseClass entity)
        {
            if (entity is UsuarioRoleUpdate usuarioRole)
            {
                var operation = new SqlOperation
                {
                    ProcedureName = "UpdateUsuarioRole"
                };
                operation.AddVarcharParam("OriginalCorreoElectronico", usuarioRole.OriginalCorreoElectronico);
                operation.AddVarcharParam("CorreoElectronico", usuarioRole.CorreoElectronico);
                operation.AddNVarcharParam("Nombre", usuarioRole.Nombre);
                operation.AddDateTimeParam("FechaRegistro", usuarioRole.FechaRegistro);
                operation.AddNVarcharParam("Telefono", usuarioRole.Telefono);
                operation.AddNVarcharParam("TipoUsuario", usuarioRole.TipoUsuario);
                operation.AddBitParam("Estado", usuarioRole.Estado);
                operation.AddBitParam("HaPagado", usuarioRole.HaPagado);
                operation.AddBitParam("CorreoVerificado", usuarioRole.CorreoVerificado);
                operation.AddBitParam("TelefonoVerificado", usuarioRole.TelefonoVerificado);
                operation.AddIntegerParam("RolId", usuarioRole.RolId);
                return operation;
            }
            else
            {
                throw new InvalidCastException("Entity is not of type UsuarioRoleUpdate");
            }
        }


        public SqlOperation GetRetrieveByEmailStatement(string correoElectronico)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetUsuarioRoleByEmail"
            };
            operation.AddVarcharParam("CorreoElectronico", correoElectronico);
            return operation;
        }

        public override SqlOperation GetRetrieveByIdStatement(int id)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetUsuarioRoleById"
            };
            operation.AddIntegerParam("Id", id);
            return operation;
        }

        public override SqlOperation GetRetrieveAllStatement()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetAllUsuariosConRoles"
            };
            return operation;
        }

        public override SqlOperation GetDeleteStatement(BaseClass entity)
        {
            var usuarioRole = (UsuarioRole)entity;
            var operation = new SqlOperation
            {
                ProcedureName = "DeleteUsuarioRole"
            };
            operation.AddVarcharParam("CorreoElectronico", usuarioRole.CorreoElectronico);
            return operation;
        }

        public override BaseClass BuildObject(Dictionary<string, object> row)
        {
            var usuarioRole = new UsuarioRole
            {
                CorreoElectronico = row["CorreoElectronico"].ToString(),
                Nombre = row.ContainsKey("Nombre") && row["Nombre"] != DBNull.Value ? row["Nombre"].ToString() : null,
                Contrasena = row.ContainsKey("Contrasena") && row["Contrasena"] != DBNull.Value ? row["Contrasena"].ToString() : null,
                FechaRegistro = row.ContainsKey("FechaRegistro") && row["FechaRegistro"] != DBNull.Value ? Convert.ToDateTime(row["FechaRegistro"]) : default(DateTime),
                Telefono = row.ContainsKey("Telefono") && row["Telefono"] != DBNull.Value ? row["Telefono"].ToString() : null,
                TipoUsuario = row.ContainsKey("TipoUsuario") && row["TipoUsuario"] != DBNull.Value ? row["TipoUsuario"].ToString() : null,
                Estado = row.ContainsKey("Estado") && row["Estado"] != DBNull.Value ? Convert.ToBoolean(row["Estado"]) : false,
                HaPagado = row.ContainsKey("HaPagado") && row["HaPagado"] != DBNull.Value ? Convert.ToBoolean(row["HaPagado"]) : false,
                CorreoVerificado = row.ContainsKey("CorreoVerificado") && row["CorreoVerificado"] != DBNull.Value ? Convert.ToBoolean(row["CorreoVerificado"]) : false,
                TelefonoVerificado = row.ContainsKey("TelefonoVerificado") && row["TelefonoVerificado"] != DBNull.Value ? Convert.ToBoolean(row["TelefonoVerificado"]) : false,
                RolId = row.ContainsKey("RolId") && row["RolId"] != DBNull.Value ? Convert.ToInt32(row["RolId"]) : 0,
                // RolNombre = row.ContainsKey("RolNombre") && row["RolNombre"] != DBNull.Value ? row["RolNombre"].ToString() : "N/A", // Eliminar esta línea si no es necesaria
                ID = row.ContainsKey("Id") && row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0
            };
            return usuarioRole;
        }




        public override List<BaseClass> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<BaseClass>();

            foreach (var row in lstRows)
            {
                var usuarioRole = BuildObject(row);
                lstResults.Add(usuarioRole);
            }
            return lstResults;
        }
    }
}
