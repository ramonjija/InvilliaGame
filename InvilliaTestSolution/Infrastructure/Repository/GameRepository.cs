using Domain.Interfaces;
using Domain.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        public GameRepository(BorrowedGamesContext context) : base(context)
        {
        }
        public async Task<Game> GetGameByName(string gameName)
        {
            return await _context.Games.FirstOrDefaultAsync(c => c.GameName.ToLower() == gameName.ToLower());
        }
        public async Task<IEnumerable<Game>> GetGamesWithBorrowed()
        {
            return await _context.Games.Include(c => c.BorrowedGame).ThenInclude(d => d.Friend).ToListAsync();
        }
        public async Task<Game> GetGameByIdWithBorrowed(int gameId)
        {
            return await _context.Games.Include(c => c.BorrowedGame).ThenInclude(d => d.Friend).FirstOrDefaultAsync(c => c.GameId == gameId);
        }
    }
}
