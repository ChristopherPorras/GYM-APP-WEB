using DataAccess.DAO;
using DataAccess.Mapper;
using DTO;
using System;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class MedidasCorporalesCrudFactory : CrudFactory
    {
        MedidasCorporalesMapper mapper;

        public MedidasCorporalesCrudFactory() : base()
        {
            mapper = new MedidasCorporalesMapper();
        }

        // Implementa el método Create utilizando el procedimiento InsertarMedidaCorporal
        public override void Create(BaseClass entity)
        {
            var medida = (MedidasCorporales)entity;
            var sqlOperation = mapper.GetCreateStatement(medida);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        // Implementa el método Update utilizando el procedimiento ActualizarMedidaCorporal
        public override void Update(BaseClass entity)
        {
            var medida = (MedidasCorporales)entity;
            var sqlOperation = mapper.GetUpdateStatement(medida);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        // Implementa el método Delete utilizando el procedimiento EliminarMedidaCorporal
        public override void Delete(BaseClass entity)
        {
            var medida = (MedidasCorporales)entity;
            var sqlOperation = mapper.GetDeleteStatement(medida. MedidasId);
            dao.ExecuteStoredProcedure(sqlOperation);
        }

        // Implementa el método Retrieve para obtener una medida corporal por Id
        public override T Retrieve<T>(int id)
        {
            var sqlOperation = mapper.GetRetrieveStatement(id);
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                var obj = mapper.BuildObject(lstResult[0]);
                return (T)Convert.ChangeType(obj, typeof(T));
            }

            return default(T);
        }

        // Implementa el método RetrieveByEmail para obtener medidas por CorreoElectronico
        public T RetrieveByEmail<T>(string correoElectronico)
        {
            var sqlOperation = mapper.GetRetrieveByEmailStatement(correoElectronico);
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                var obj = mapper.BuildObject(lstResult[0]);
                return (T)Convert.ChangeType(obj, typeof(T));
            }

            return default(T);
        }

        // Implementa el método RetrieveAll para obtener todas las medidas corporales
        public override List<T> RetrieveAll<T>()
        {
            var lstMedidas = new List<T>();

            var sqlOperation = mapper.GetRetrieveAllStatement();
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                var objs = mapper.BuildObjects(lstResult);
                foreach (var obj in objs)
                {
                    lstMedidas.Add((T)Convert.ChangeType(obj, typeof(T)));
                }
            }

            return lstMedidas;
        }

        // Implementa el método RetrieveById para obtener una medida corporal por Id (versión alternativa)
        public override BaseClass RetrieveById(int id)
        {
            var sqlOperation = mapper.GetRetrieveStatement(id);
            var lstResult = dao.ExecuteStoredProcedureWithQuery(sqlOperation);

            if (lstResult.Count > 0)
            {
                return mapper.BuildObject(lstResult[0]);
            }

            return null;
        }
    }
}
