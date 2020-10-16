using Domain.Model.Aggregate;
using Domain.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBorrowGameService
    {
        Task<IServiceResult<IList<BorrowedGame>>> GetBorrowGamesOfUser(int friendId);
        Task<IServiceResult<BorrowedGame>> BorrowGame(int friendId, int gameId);
        Task<IServiceResult<IList<BorrowedGame>>> BorrowGames(int friendId, List<int> gameIds);
        Task<IServiceResult<IList<BorrowedGame>>> ReturnGames(int friendId, List<int> borrowedGamesIds);


    }
}
