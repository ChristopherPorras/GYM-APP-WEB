using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class CitaCrudFactory : CrudFactory
    {
        private readonly CitaMapper mapper;
        private readonly SqlDao dao;

        public CitaCrudFactory()
        {
            mapper = new CitaMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseClass entityDTO)
        {
            var cita = (Cita)entityDTO;
            var sqlOperation = mapper.GetCreateStatement(cita);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Update(BaseClass entityDTO)
        {
            var cita = (Cita)entityDTO;
            var sqlOperation = mapper.GetUpdateStatement(cita);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        public override void Delete(BaseClass entityDTO)
        {
            var cita = (Cita)entityDTO;
            var sqlOperation = mapper.GetDeleteStatement(cita);
            dao.ExecuteStoredProcedure(sqlOperation);
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

        public override BaseClass RetrieveById(int id)
        {
            var sqlOperation = mapper.GetRetrieveByIdStatement(id);
            var result = dao.ExecuteStoredProcedureWithQuery(sqlOperation);
            if (result.Count > 0)
            {
                return mapper.BuildObject(result[0]);
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

        public async Task<List<Cita>> RetrieveByEntrenadorCorreoAsync(string entrenadorCorreo)
        {
            var lstResult = new List<Cita>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RetrieveCitasByEntrenadorCorreo"
            };
            sqlOperation.AddVarcharParam("EntrenadorCorreo", entrenadorCorreo);

            var lstResultQuery = await dao.ExecuteStoredProcedureWithQueryAsync(sqlOperation);
            foreach (var row in lstResultQuery)
            {
                var cita = new Cita
                {
                    ID = Convert.ToInt32(row["Id"]),
                    CorreoElectronico = row["CorreoElectronico"].ToString(),
                    EntrenadorCorreo = row["EntrenadorCorreo"].ToString(),
                    FechaCita = Convert.ToDateTime(row["FechaCita"]),
                    Estado = row["Estado"].ToString()
                };
                lstResult.Add(cita);
            }
            return lstResult;
        }

        public List<Cita> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var citas = new List<Cita>();

            foreach (var row in lstRows)
            {
                var cita = (Cita)mapper.BuildObject(row);
                citas.Add(cita);
            }

            return citas;
        }

        public Cita BuildObject(Dictionary<string, object> row)
        {
            return (Cita)mapper.BuildObject(row);
        }

        public SqlOperation GetCreateStatement(Cita cita)
        {
            return mapper.GetCreateStatement(cita);
        }

        public SqlOperation GetUpdateStatement(Cita cita)
        {
            return mapper.GetUpdateStatement(cita);
        }

        public SqlOperation GetDeleteStatement(Cita cita)
        {
            return mapper.GetDeleteStatement(cita);
        }

        public void ExecuteStoredProcedure(SqlOperation operation)
        {
            dao.ExecuteStoredProcedure(operation);
        }

        public async Task ExecuteStoredProcedureAsync(SqlOperation operation)
        {
            await dao.ExecuteStoredProcedureAsync(operation);
        }

        public List<Dictionary<string, object>> ExecuteStoredProcedureWithQuery(SqlOperation operation)
        {
            return dao.ExecuteStoredProcedureWithQuery(operation);
        }

        public async Task<List<Dictionary<string, object>>> ExecuteStoredProcedureWithQueryAsync(SqlOperation operation)
        {
            return await dao.ExecuteStoredProcedureWithQueryAsync(operation);
        }
    }
}
