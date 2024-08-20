using DataAccess.CRUD;
using DataAccess.DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public class CitaManager
    {
        private readonly CitaCrudFactory citaCrudFactory;

        public CitaManager()
        {
            citaCrudFactory = new CitaCrudFactory();
        }

        public void CreateCita(Cita cita)
        {
            citaCrudFactory.Create(cita);
        }

        public async Task CreateCitaAsync(Cita cita)
        {
            var operation = citaCrudFactory.GetCreateStatement(cita);
            await citaCrudFactory.ExecuteStoredProcedureAsync(operation);
        }

        public List<Cita> GetCitasByUsuarioID(string correoElectronico)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetCitasMedicionCorporalByUsuario"
            };
            operation.AddVarcharParam("CorreoElectronico", correoElectronico);

            var result = citaCrudFactory.ExecuteStoredProcedureWithQuery(operation);
            return citaCrudFactory.BuildObjects(result);
        }

        public async Task<List<Cita>> GetCitasByUsuarioIDAsync(string correoElectronico)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetCitasMedicionCorporalByUsuario"
            };
            operation.AddVarcharParam("CorreoElectronico", correoElectronico);

            var result = await citaCrudFactory.ExecuteStoredProcedureWithQueryAsync(operation);
            return citaCrudFactory.BuildObjects(result);
        }

        public void UpdateCita(Cita cita)
        {
            var operation = citaCrudFactory.GetUpdateStatement(cita);
            citaCrudFactory.ExecuteStoredProcedure(operation);
        }

        public async Task UpdateCitaAsync(Cita cita)
        {
            var operation = citaCrudFactory.GetUpdateStatement(cita);
            await citaCrudFactory.ExecuteStoredProcedureAsync(operation);
        }

        public void DeleteCita(string correoElectronico, DateTime fechaCita)
        {
            var cita = new Cita
            {
                CorreoElectronico = correoElectronico,
                FechaCita = fechaCita
            };
            var operation = citaCrudFactory.GetDeleteStatement(cita);
            citaCrudFactory.ExecuteStoredProcedure(operation);
        }

        public async Task DeleteCitaAsync(string correoElectronico, DateTime fechaCita)
        {
            var cita = new Cita
            {
                CorreoElectronico = correoElectronico,
                FechaCita = fechaCita
            };
            var operation = citaCrudFactory.GetDeleteStatement(cita);
            await citaCrudFactory.ExecuteStoredProcedureAsync(operation);
        }

        public async Task<List<Cita>> GetAllCitasAsync()
        {
            var operation = new SqlOperation { ProcedureName = "GetAllCitasMedicionCorporal" };
            var result = await citaCrudFactory.ExecuteStoredProcedureWithQueryAsync(operation);
            return citaCrudFactory.BuildObjects(result);
        }

        public async Task<List<Cita>> GetCitasByEntrenadorAsync(string entrenadorCorreo)
        {
            return await citaCrudFactory.RetrieveByEntrenadorCorreoAsync(entrenadorCorreo);
        }

        // Métodos adicionales
        public async Task<Cita> GetCitaByDateAndUserAsync(string correoElectronico, DateTime fechaCita)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetCitaByDateAndUser"
            };
            operation.AddVarcharParam("CorreoElectronico", correoElectronico);
            operation.AddDateTimeParam("FechaCita", fechaCita);

            var result = await citaCrudFactory.ExecuteStoredProcedureWithQueryAsync(operation);
            return citaCrudFactory.BuildObject(result.Count > 0 ? result[0] : null);
        }

        public async Task<List<Cita>> GetCitaAgendadaByUserAsync(string correoElectronico)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetCitasAgendadasByUsuario"
            };
            operation.AddVarcharParam("CorreoElectronico", correoElectronico);

            var result = await citaCrudFactory.ExecuteStoredProcedureWithQueryAsync(operation);
            return citaCrudFactory.BuildObjects(result);
        }

    }
}
