using Domain.Interfaces;
using Domain.Model.Aggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BorrowedGameRepository : BaseRepository<BorrowedGame>, IBorrowedGameRepository
    {
        public BorrowedGameRepository(BorrowedGamesContext options) : base(options)
        {
        }
        public async Task<BorrowedGame> GetBorrowedGameByUserId(int userId, int gameId)
        {
            return await _context.BorrowedGames.Include(c => c.Game).FirstOrDefaultAsync(c => c.Friend.UserId == userId && c.Game.GameId == gameId);
        }

        public async Task<IList<BorrowedGame>> GetBorrowedGamesByUserId(int userId)
        {
            return await _context.BorrowedGames.Include(c => c.Game).Where(c => c.Friend.UserId == userId).ToListAsync();
        }

        public async Task<IList<BorrowedGame>> GetBorrowedGamesById(List<int> borrowedGamesIds)
        {
            return await _context.BorrowedGames
                            .Include(c => c.Friend)
                            .Include(c => c.Game)
                            .Where(c => borrowedGamesIds.Contains(c.BorrowedGameId))
                            .ToListAsync();
        }

        public async Task<BorrowedGame> GetBorrowedGameById(int borrowedGameId)
        {
            return await _context.BorrowedGames
                            .Include(c => c.Friend)
                            .Include(c => c.Game)
                            .Where(c => c.BorrowedGameId == borrowedGameId)
                            .FirstOrDefaultAsync();
        }

    }
}
