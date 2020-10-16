using Domain.Interfaces;
using Domain.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGameService
    {
        public Task<IServiceResult<Game>> CreateGame(string name);
        public Task<IServiceResult<Game>> GetGame(int gameId);
        public Task<IServiceResult<Game>> GetGame(string gameName);
        public Task<IServiceResult<IEnumerable<Game>>> GetGame();
        public Task<IServiceResult<Game>> UpdateGame(int gameId, string gameName);
        public Task<IServiceResult<Game>> DeleteGame(int gameId);
    }
}
