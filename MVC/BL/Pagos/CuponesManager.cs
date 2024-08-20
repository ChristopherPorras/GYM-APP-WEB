using DataAccess.CRUD;
using DTO;
using System.Collections.Generic;

namespace BL
{
    public class CuponesManager
    {
        private readonly CuponesCrudFactory _crudFactory;

        public CuponesManager()
        {
            _crudFactory = new CuponesCrudFactory();
        }

        public void CreateCupon(Cupones cupon)
        {
            _crudFactory.Create(cupon);
        }

        public void ApplyCupon(string codigo, string correoElectronico)
        {
            _crudFactory.ApplyCoupon(codigo, correoElectronico);
        }
    }
}
