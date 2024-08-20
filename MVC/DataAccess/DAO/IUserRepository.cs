using System.Collections.Generic;
using DTO;

namespace DataAccess.DAO
{
    public interface IUserRepository
    {
        List<User> GetAllEntrenadores();
        User GetByEmail(string email);
    }
}
