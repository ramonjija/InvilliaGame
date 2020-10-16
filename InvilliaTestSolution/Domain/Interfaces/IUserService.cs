using Domain.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        public Task<IServiceResult<User>> CreateUser(string name, string passwordHash, int userTypeId);
        public Task<IServiceResult<User>> GetUser(int userId);
        public Task<IServiceResult<User>> GetUser(string userName, string password);
        public Task<IServiceResult<IEnumerable<User>>> GetUser();
        public Task<IServiceResult<User>> UpdateUser(int userId, string userName, string password, int TypeId);
        public Task<IServiceResult<User>> DeleteUser(int userId);
    }
}
