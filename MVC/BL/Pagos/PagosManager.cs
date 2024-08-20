using DataAccess.CRUD;
using DTO;
using System.Collections.Generic;

namespace BL
{
    public class PagosManager
    {
        private PagosCrudFactory _crudFactory;

        public PagosManager()
        {
            _crudFactory = new PagosCrudFactory();
        }

        public void CreatePago(Pagos pago)
        {
            _crudFactory.Create(pago);
        }

        public List<Pagos> RetrieveAllPagos()
        {
            return _crudFactory.RetrieveAll<Pagos>();
        }

        public Pagos RetrievePagoById(int id)
        {
            return _crudFactory.Retrieve<Pagos>(id);
        }

        public void UpdatePago(Pagos pago)
        {
            _crudFactory.Update(pago);
        }

        public void DeletePago(int id)
        {
            var pago = new Pagos { ID = id };
            _crudFactory.Delete(pago);
        }
    }
}
