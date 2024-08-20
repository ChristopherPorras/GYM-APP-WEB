using DTO;
using DataAccess.DAO;
using System;
using System.Collections.Generic;

namespace BL
{
    public class AccesoManager
    {
        public void CreateAcceso(Acceso acceso)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "CreateAcceso"
            };
            operation.AddVarcharParam("Nombre", acceso.Nombre);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public List<Acceso> GetAllAccesos()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetAllAccesos"
            };

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            var accesos = new List<Acceso>();
            foreach (var row in result)
            {
                var acceso = new Acceso
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Nombre = row["Nombre"].ToString()
                };
                accesos.Add(acceso);
            }

            return accesos;
        }

        public void AssignAccessToRol(int rolID, int accesoID)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "AssignAccessToRol"
            };
            operation.AddIntegerParam("RolID", rolID);
            operation.AddIntegerParam("AccesoID", accesoID);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public void RemoveAccessFromRol(int rolID, int accesoID)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "RemoveAccessFromRol"
            };
            operation.AddIntegerParam("RolID", rolID);
            operation.AddIntegerParam("AccesoID", accesoID);

            var dao = SqlDao.GetInstance();
            dao.ExecuteStoredProcedure(operation);
        }

        public List<Acceso> GetAccessByRol(int rolID)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetAccessByRol"
            };
            operation.AddIntegerParam("RolID", rolID);

            var dao = SqlDao.GetInstance();
            var result = dao.ExecuteStoredProcedureWithQuery(operation);

            var accesos = new List<Acceso>();
            foreach (var row in result)
            {
                var acceso = new Acceso
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Nombre = row["Nombre"].ToString()
                };
                accesos.Add(acceso);
            }

            return accesos;
        }
    }
}
