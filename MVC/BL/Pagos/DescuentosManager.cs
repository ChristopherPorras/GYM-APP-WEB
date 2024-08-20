using DataAccess.CRUD;
using DTO;

namespace BL
{
    public class DescuentosManager : BaseClass
    {
        private readonly DescuentosCrudFactory crudDescuento;

        public DescuentosManager()
        {
            crudDescuento = new DescuentosCrudFactory();
        }

        public void CreateDescuento(Descuentos descuento)
        {
            crudDescuento.Create(descuento);
        }

        public List<Descuentos> RetrieveAllDescuentos()
        {
            return crudDescuento.RetrieveAll<Descuentos>();
        }

        public Descuentos RetrieveDescuentoById(int id)
        {
            return crudDescuento.Retrieve<Descuentos>(id);
        }

        public void UpdateDescuento(Descuentos descuento)
        {
            crudDescuento.Update(descuento);
        }

        public void DeleteDescuento(int id)
        {
            var descuento = new Descuentos { ID = id };
            crudDescuento.Delete(descuento);
        }
    }
}
