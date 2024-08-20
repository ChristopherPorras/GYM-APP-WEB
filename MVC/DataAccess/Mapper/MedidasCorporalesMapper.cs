using DataAccess.DAO;
using DTO;
using System;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class MedidasCorporalesMapper
    {
        // Construye un objeto MedidasCorporalesDTO a partir de un diccionario
        public MedidasCorporales BuildObject(Dictionary<string, object> row)
        {
            return new MedidasCorporales
            {
                MedidasId = Convert.ToInt32(row["Id"]),
                CorreoElectronico = row["CorreoElectronico"].ToString(),
                FechaMedicion = Convert.ToDateTime(row["FechaMedicion"]),
                Peso = Convert.ToDecimal(row["Peso"]),
                Altura = Convert.ToDecimal(row["Altura"]),
                EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
            };
        }

        // Construye una lista de objetos MedidasCorporalesDTO a partir de una lista de diccionarios
        public List<MedidasCorporales> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<MedidasCorporales>();

            foreach (var row in lstRows)
            {
                var medida = BuildObject(row);
                lstResults.Add(medida);
            }

            return lstResults;
        }

        // Genera una operación SQL para insertar una nueva medida corporal
        public SqlOperation GetCreateStatement(MedidasCorporales medida)
        {
            var operation = new SqlOperation { ProcedureName = "InsertarMedidaCorporal" };

            operation.AddVarcharParam("CorreoElectronico", medida.CorreoElectronico);
            operation.AddDateTimeParam("FechaMedicion", medida.FechaMedicion);
            operation.AddDecimalParam("Peso", medida.Peso);
            operation.AddDecimalParam("Altura", medida.Altura);
            operation.AddVarcharParam("EntrenadorCorreo", medida.EntrenadorCorreo);

            return operation;
        }

        // Genera una operación SQL para obtener una medida corporal por Id
        public SqlOperation GetRetrieveStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "ObtenerMedidasCorporalesPorId" };

            operation.AddIntegerParam("Id", id);
            return operation;
        }

        // Genera una operación SQL para obtener medidas corporales por correo electrónico
        public SqlOperation GetRetrieveByEmailStatement(string correo)
        {
            var operation = new SqlOperation { ProcedureName = "ObtenerMedidasPorCorreo" };
            operation.AddVarcharParam("CorreoElectronico", correo);
            return operation;
        }

        // Genera una operación SQL para obtener todas las medidas corporales
        public SqlOperation GetRetrieveAllStatement()
        {
            return new SqlOperation { ProcedureName = "ObtenerMedidasCorporales" };
        }

        // Genera una operación SQL para actualizar una medida corporal existente
        public SqlOperation GetUpdateStatement(MedidasCorporales medida)
        {
            var operation = new SqlOperation { ProcedureName = "ActualizarMedidaCorporal" };

            operation.AddIntegerParam("Id", medida.MedidasId);
            operation.AddVarcharParam("CorreoElectronico", medida.CorreoElectronico);
            operation.AddDateTimeParam("FechaMedicion", medida.FechaMedicion);
            operation.AddDecimalParam("Peso", medida.Peso);
            operation.AddDecimalParam("Altura", medida.Altura);
            operation.AddVarcharParam("EntrenadorCorreo", medida.EntrenadorCorreo);

            return operation;
        }

        // Genera una operación SQL para eliminar una medida corporal por Id
        public SqlOperation GetDeleteStatement(int id)
        {
            var operation = new SqlOperation { ProcedureName = "EliminarMedidaCorporal" };

            operation.AddIntegerParam("Id", id);
            return operation;
        }
    }
}
