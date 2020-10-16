using Domain.Interfaces;
using Domain.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserWithType(int userId);
        Task<IEnumerable<User>> GetUsersWithType();
        Task<User> GetUserWithType(string userName, string password);
        Task<User> GetUserByName(string userName);
    }
}
