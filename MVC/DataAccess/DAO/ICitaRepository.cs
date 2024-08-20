using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace DataAccess.DAO
{
    public interface ICitaRepository
    {
        void Create(Cita cita);
        List<Cita> GetByUsuarioID(string correoElectronico);
        void Update(Cita cita);
        void Delete(int id);
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<IEnumerable<Cita>> GetByEntrenadorAsync(string correoElectronico);
    }
}
