using Domain.Interfaces;
using Domain.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class GameService : IGameService
    {
        private IUnityOfWork _unitOfWork;
        public GameService(IUnityOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IServiceResult<Game>> CreateGame(string name)
        {
            try
            {
                var serviceResult = new ServiceResult<Game>();
                var existingGame = await _unitOfWork.Games.GetGameByName(name);
                if (existingGame != null)
                {
                    serviceResult.AddMessage($"There's already a Game with this name. 'Name: {name}'");
                }

                if (!serviceResult.Success)
                    return serviceResult;

                var newGame = await _unitOfWork.Games.Create(new Game(name));
                await _unitOfWork.Commit();
                serviceResult.SetResult(newGame);

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IServiceResult<Game>> DeleteGame(int gameId)
        {
            try
            {
                var serviceResult = new ServiceResult<Game>();
                var deletedGame = await _unitOfWork.Games.Delete(gameId);
                if (deletedGame == null)
                {
                    serviceResult.AddMessage("Game Not Found");
                    return serviceResult;
                }
                await _unitOfWork.Commit();
                serviceResult.SetResult(deletedGame);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IServiceResult<IEnumerable<Game>>> GetGame()
        {
            try
            {
                var serviceResult = new ServiceResult<IEnumerable<Game>>();
                var Games = await _unitOfWork.Games.GetGamesWithBorrowed();
                serviceResult.SetResult(Games);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IServiceResult<Game>> GetGame(int gameId)
        {
            try
            {
                var serviceResult = new ServiceResult<Game>();
                var Game = await _unitOfWork.Games.GetGameByIdWithBorrowed(gameId);
                if (Game == null)
                {
                    serviceResult.AddMessage("Game Not Found");
                    return serviceResult;
                }
                serviceResult.SetResult(Game);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IServiceResult<Game>> GetGame(string gameName)
        {
            try
            {
                var serviceResult = new ServiceResult<Game>();
                var Game = await _unitOfWork.Games.GetGameByName(gameName);
                if (Game == null)
                {
                    serviceResult.AddMessage("Game Not Found");
                    return serviceResult;
                }
                serviceResult.SetResult(Game);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IServiceResult<Game>> UpdateGame(int gameId, string gameName)
        {
            try
            {
                var serviceResult = new ServiceResult<Game>();
                var Game = await _unitOfWork.Games.GetById(gameId);
                if (Game == null)
                    serviceResult.AddMessage($"Game Not Found '{gameName}'");

                if (!serviceResult.Success)
                    return serviceResult;

                Game.Update(gameName);

                var updatedGame = _unitOfWork.Games.Update(Game);
                await _unitOfWork.Commit();

                serviceResult.SetResult(updatedGame);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
