using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public abstract class BaseManager
    {
        // Implementación genérica que otros managers pueden heredar
        public virtual void Create(BaseClass entity) { }
        public virtual void Update(BaseClass entity) { }
        public virtual void Delete(BaseClass entity) { }
        public virtual T Retrieve<T>(int id) where T : class { return null; }
        public virtual List<T> RetrieveAll<T>() where T : class { return null; }
    }
}
