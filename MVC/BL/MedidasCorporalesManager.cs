using DTO;
using DataAccess.DAO;
using System;
using System.Collections.Generic;

namespace BL
{
    public class MedidasCorporalesManager
    {
        public void CreateMedidaCorporal(MedidasCorporales medida)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "InsertarMedidaCorporal"
            };
            operation.AddVarcharParam("CorreoElectronico", medida.CorreoElectronico);
            operation.AddDateTimeParam("FechaMedicion", medida.FechaMedicion);
            operation.AddDecimalParam("Peso", medida.Peso);
            operation.AddDecimalParam("Altura", medida.Altura);
            operation.AddVarcharParam("EntrenadorCorreo", medida.EntrenadorCorreo);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public List<MedidasCorporales> GetMedidasCorporales()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "ObtenerMedidasCorporales"
            };

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            var medidas = new List<MedidasCorporales>();
            foreach (var row in result)
            {
                var medida = new MedidasCorporales
                {
                    MedidasId = Convert.ToInt32(row["Id"]),
                    CorreoElectronico = row["CorreoElectronico"].ToString(),
                    FechaMedicion = Convert.ToDateTime(row["FechaMedicion"]),
                    Peso = Convert.ToDecimal(row["Peso"]),
                    Altura = Convert.ToDecimal(row["Altura"]),
                    EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
                };
                medidas.Add(medida);
            }

            return medidas;
        }

        public MedidasCorporales GetMedidaCorporalById(int id)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "ObtenerMedidasCorporalesPorId"
            };
            operation.AddIntegerParam("Id", id);

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            if (result.Count > 0)
            {
                var row = result[0];
                var medida = new MedidasCorporales
                {
                    MedidasId = Convert.ToInt32(row["Id"]),
                    CorreoElectronico = row["CorreoElectronico"].ToString(),
                    FechaMedicion = Convert.ToDateTime(row["FechaMedicion"]),
                    Peso = Convert.ToDecimal(row["Peso"]),
                    Altura = Convert.ToDecimal(row["Altura"]),
                    EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
                };
                return medida;
            }

            return null;
        }

        public MedidasCorporales GetMedidaCorporalByCorreo(string correoElectronico)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "ObtenerMedidasPorCorreo"
            };
            operation.AddVarcharParam("CorreoElectronico", correoElectronico);

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            if (result.Count > 0)
            {
                var row = result[0];
                var medida = new MedidasCorporales
                {
                    MedidasId = Convert.ToInt32(row["Id"]),
                    CorreoElectronico = row["CorreoElectronico"].ToString(),
                    FechaMedicion = Convert.ToDateTime(row["FechaMedicion"]),
                    Peso = Convert.ToDecimal(row["Peso"]),
                    Altura = Convert.ToDecimal(row["Altura"]),
                    EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
                };
                return medida;
            }

            return null;
        }

        public void UpdateMedidaCorporal(MedidasCorporales medida)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "ActualizarMedidaCorporal"
            };
            operation.AddIntegerParam("Id", medida. MedidasId);
            operation.AddVarcharParam("CorreoElectronico", medida.CorreoElectronico);
            operation.AddDateTimeParam("FechaMedicion", medida.FechaMedicion);
            operation.AddDecimalParam("Peso", medida.Peso);
            operation.AddDecimalParam("Altura", medida.Altura);
            operation.AddVarcharParam("EntrenadorCorreo", medida.EntrenadorCorreo);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public void DeleteMedidaCorporal(int id)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "EliminarMedidaCorporal"
            };
            operation.AddIntegerParam("Id", id);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }
    }
}