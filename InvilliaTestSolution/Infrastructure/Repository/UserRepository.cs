using Domain.Interfaces;
using Domain.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BorrowedGamesContext context) : base(context)
        {
        } 
        public async Task<User> GetUserWithType(int userId)
        {
           return await _context.Users.Include(c => c.UserType).FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<IEnumerable<User>> GetUsersWithType()
        {
            return await _context.Users.Include(c => c.UserType).ToListAsync();
        }

        public async Task<User> GetUserWithType(string userName, string password)
        {
            return await _context.Users.Include(c => c.UserType).FirstOrDefaultAsync(d => d.UserName == userName);
        }

        public async Task<User> GetUserByName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.UserName.ToLower() == userName.ToLower());
        }
    }
}
