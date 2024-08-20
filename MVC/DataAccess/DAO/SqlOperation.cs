using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DAO
{
    public class SqlOperation
    {
        public string ProcedureName { get; set; }
        public List<SqlParameter> parameters { get; set; }

        public SqlOperation()
        {
            parameters = new List<SqlParameter>();
        }

        public void AddVarcharParam(string parameterName, string paramValue)
        {
            parameters.Add(new SqlParameter("@" + parameterName, paramValue));
        }

        public void AddIntegerParam(string parameterName, int paramValue)
        {
            parameters.Add(new SqlParameter("@" + parameterName, paramValue));
        }

        public void AddDateTimeParam(string parameterName, DateTime paramValue)
        {
            parameters.Add(new SqlParameter("@" + parameterName, paramValue));
        }

        public void AddDecimalParam(string parameterName, decimal paramValue)
        {
            parameters.Add(new SqlParameter("@" + parameterName, SqlDbType.Decimal) { Value = paramValue });
        }

        public void AddBitParam(string parameterName, bool paramValue)
        {
            parameters.Add(new SqlParameter("@" + parameterName, SqlDbType.Bit) { Value = paramValue });
        }

        public void AddBinaryParam(string parameterName, byte[] value)
        {
            var parameter = new SqlParameter
            {
                ParameterName = "@" + parameterName,
                SqlDbType = SqlDbType.VarBinary,
                Value = value ?? (object)DBNull.Value
            };
            parameters.Add(parameter);
        }

        public void AddNVarcharParam(string parameterName, string paramValue)
        {
            parameters.Add(new SqlParameter("@" + parameterName, SqlDbType.NVarChar) { Value = paramValue });
        }
    }
}
