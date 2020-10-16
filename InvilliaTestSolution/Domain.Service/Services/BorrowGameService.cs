using Domain.Interfaces;
using Domain.Model.Aggregate;
using Domain.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service.Services
{
    public class BorrowGameService : IBorrowGameService
    {
        IUnityOfWork _unitOfWork;
        public BorrowGameService(IUnityOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IServiceResult<IList<BorrowedGame>>> GetBorrowGamesOfUser(int friendId)
        {
            try
            {
                var serviceResult = new ServiceResult<IList<BorrowedGame>>();
                var borrowedGames = await _unitOfWork.BorrowedGames.GetBorrowedGamesByUserId(friendId);
                serviceResult.SetResult(borrowedGames);

                return serviceResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IServiceResult<BorrowedGame>> BorrowGame(int friendId, int gameId)
        {
            try
            {
                var serviceResult = new ServiceResult<BorrowedGame>();
                var user = await _unitOfWork.Users.GetUserWithType(friendId);
                if (user == null)
                {
                    serviceResult.AddMessage("User Not Found");
                    return serviceResult;
                }
                var game = await _unitOfWork.Games.GetGameByIdWithBorrowed(gameId);
                if (game == null)
                {
                    serviceResult.AddMessage("Game Not Found");
                    return serviceResult;
                }
                if (!game.Available)
                {
                    serviceResult.AddMessage("Game Not Available");
                    return serviceResult;
                }

                var borrowedGame = await _unitOfWork.BorrowedGames.Create(new BorrowedGame((Friend)user, game));
                await _unitOfWork.Commit();
                serviceResult.SetResult(borrowedGame);

                return serviceResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IServiceResult<IList<BorrowedGame>>> BorrowGames(int friendId, List<int> gameIds)
        {
            try
            {
                var serviceResult = new ServiceResult<IList<BorrowedGame>>();
                var borrowedGames = new List<BorrowedGame>();
                var user = await _unitOfWork.Users.GetUserWithType(friendId);
                if (user == null)
                {
                    serviceResult.AddMessage("User Not Found");
                    return serviceResult;
                }

                foreach (var gameId in gameIds)
                {
                    var game = await _unitOfWork.Games.GetGameByIdWithBorrowed(gameId);
                    if (game == null)
                    {
                        serviceResult.AddMessage("Game Not Found");
                        return serviceResult;
                    }
                    if (!game.Available)
                    {
                        serviceResult.AddMessage("Game Not Available");
                        return serviceResult;
                    }
                    var borrowedGame = await _unitOfWork.BorrowedGames.Create(new BorrowedGame((Friend)user, game));
                    borrowedGames.Add(borrowedGame);
                }
                await _unitOfWork.Commit();
                serviceResult.SetResult(borrowedGames);
                return serviceResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IServiceResult<BorrowedGame>> ReturnGame(int friendId, int borrowedGameId)
        {
            try
            {
                var serviceResult = new ServiceResult<BorrowedGame>();
                var gameToBeReturned = await _unitOfWork.BorrowedGames.GetBorrowedGameById(borrowedGameId);
                if(gameToBeReturned.Friend.UserId != friendId) { 
                    serviceResult.AddMessage($"Only Games You Borrow Can Be Returned:  {borrowedGameId}");
                    return serviceResult;
                }
                
                var returnedGame = await _unitOfWork.BorrowedGames.Delete(borrowedGameId);
                if (returnedGame == null)
                {
                    serviceResult.AddMessage($"Game couldn't be returned: {borrowedGameId}");
                    return serviceResult;

                }
                returnedGame.Game.Return();
                _unitOfWork.Games.Update(returnedGame.Game);
                await _unitOfWork.Commit();

                serviceResult.SetResult(returnedGame);

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IServiceResult<IList<BorrowedGame>>> ReturnGames(int friendId, List<int> borrowedGamesIds)
        {
            try
            {
                var serviceResult = new ServiceResult<IList<BorrowedGame>>();
                var borrowedGames = await _unitOfWork.BorrowedGames.GetBorrowedGamesById(borrowedGamesIds);
                foreach (var borrowedGame in borrowedGames)
                {
                    var gameToBeReturned = await _unitOfWork.BorrowedGames.GetBorrowedGameById(borrowedGame.BorrowedGameId);
                    if (gameToBeReturned.Friend.UserId != friendId)
                    {
                        serviceResult.AddMessage($"Only Games You Borrow Can Be Returned: {borrowedGame.BorrowedGameId}");
                        return serviceResult;
                    }
                    
                    var returnedGame = await _unitOfWork.BorrowedGames.Delete(borrowedGame.BorrowedGameId);
                    if (returnedGame == null)
                    {
                        serviceResult.AddMessage($"Game couldn't be returned: {borrowedGame.BorrowedGameId}");
                        return serviceResult;
                    }
                    else
                    {
                        returnedGame.Game.Return();
                        _unitOfWork.Games.Update(returnedGame.Game);
                    }
                }
                await _unitOfWork.Commit();

                serviceResult.SetResult(borrowedGames);

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
