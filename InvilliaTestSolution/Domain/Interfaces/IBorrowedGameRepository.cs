using Domain.Model.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBorrowedGameRepository : IBaseRepository<BorrowedGame>
    {
        Task<BorrowedGame> GetBorrowedGameByUserId(int userId, int gameId);
        Task<IList<BorrowedGame>> GetBorrowedGamesByUserId(int userId);
        Task<BorrowedGame> GetBorrowedGameById(int borrowedGameId);
        Task<IList<BorrowedGame>> GetBorrowedGamesById(List<int> borrowedGamesIds);
    }
}
