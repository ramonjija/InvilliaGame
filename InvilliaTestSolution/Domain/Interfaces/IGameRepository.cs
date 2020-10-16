using Domain.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        public Task<Game> GetGameByName(string gameName);
        public Task<Game> GetGameByIdWithBorrowed(int gameId);
        Task<IEnumerable<Game>> GetGamesWithBorrowed();

    }
}
