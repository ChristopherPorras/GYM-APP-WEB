using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SqlDao
    {
        private static SqlDao instance = new SqlDao();

        private string _connString = "Data Source=SQL8005.site4now.net;Initial Catalog=db_aaaf3a_gymproyect2;User Id=db_aaaf3a_gymproyect2_admin;Password=Admin123;TrustServerCertificate=True;";

        public static SqlDao GetInstance()
        {
            if (instance == null)
                instance = new SqlDao();
            return instance;
        }

        public void ExecuteStoredProcedure(SqlOperation operation)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(operation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var p in operation.parameters)
                {
                    command.Parameters.Add(p);
                }

                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Dictionary<string, object>> ExecuteStoredProcedureWithQuery(SqlOperation operation)
        {
            List<Dictionary<string, object>> lstResults = new List<Dictionary<string, object>>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(operation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var p in operation.parameters)
                {
                    command.Parameters.Add(p);
                }

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> diccObj = new Dictionary<string, object>();

                        for (var fieldCount = 0; fieldCount < reader.FieldCount; fieldCount++)
                        {
                            diccObj.Add(reader.GetName(fieldCount), reader.GetValue(fieldCount));
                        }
                        lstResults.Add(diccObj);
                    }
                }
            }
            return lstResults;
        }

        public Dictionary<string, object> ExecuteStoredProcedureWithUniqueResult(SqlOperation operation)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(operation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var p in operation.parameters)
                {
                    command.Parameters.Add(p);
                }

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    Dictionary<string, object> diccObj = new Dictionary<string, object>();

                    for (var fieldCount = 0; fieldCount < reader.FieldCount; fieldCount++)
                    {
                        diccObj.Add(reader.GetName(fieldCount), reader.GetValue(fieldCount));
                    }
                    return diccObj;
                }
            }
            return null;
        }

        public async Task ExecuteStoredProcedureAsync(SqlOperation operation)
        {
            using (var connection = new SqlConnection(_connString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(operation.ProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var param in operation.parameters)
                    {
                        command.Parameters.Add(param);
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Dictionary<string, object>>> ExecuteStoredProcedureWithQueryAsync(SqlOperation operation)
        {
            var results = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_connString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(operation.ProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var param in operation.parameters)
                    {
                        command.Parameters.Add(param);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }

                            results.Add(row);
                        }
                    }
                }
            }

            return results;
        }

        // Métodos adicionales necesarios para el funcionamiento de tu CRUD

        public object ExecuteScalarProcedure(SqlOperation operation)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(operation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var p in operation.parameters)
                {
                    command.Parameters.Add(p);
                }

                conn.Open();
                return command.ExecuteScalar();
            }
        }

        public DataTable ExecuteQueryProcedure(SqlOperation operation)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(operation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var p in operation.parameters)
                {
                    command.Parameters.Add(p);
                }

                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void ExecuteProcedure(SqlOperation operation)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(operation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var p in operation.parameters)
                {
                    command.Parameters.Add(p);
                }

                conn.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
