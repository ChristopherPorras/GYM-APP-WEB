using DataAccess.CRUD;
using DTO;
using System.Collections.Generic;

namespace BL
{
    public class RutinaManager : BaseManager
    {
        private readonly RutinaCrudFactory _crudFactory;

        public RutinaManager()
        {
            _crudFactory = new RutinaCrudFactory();
        }

        public void Create(RutinaDTO rutina)
        {
            _crudFactory.Create(rutina);
        }

        public T RetrieveById<T>(int id)
        {
            return _crudFactory.Retrieve<T>(id);
        }

        public List<T> RetrieveAll<T>()
        {
            return _crudFactory.RetrieveAll<T>();
        }

        public void Update(RutinaDTO rutina)
        {
            _crudFactory.Update(rutina);
        }

        public void Delete(RutinaDTO rutina)
        {
            _crudFactory.Delete(rutina);
        }

        // Método para obtener rutinas por correo electrónico
        public List<T> RetrieveByCorreo<T>(string correo)
        {
            return _crudFactory.RetrieveByCorreo<T>(correo);
        }
    }
}
