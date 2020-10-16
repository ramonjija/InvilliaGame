using Domain.Interfaces;
using Domain.Model.Aggregate;
using Domain.Model.Entity;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnityOfWork : IUnityOfWork
    {

        private BorrowedGamesContext _dbcontext;
        private UserRepository _users;
        private GameRepository _games;
        private BorrowedGameRepository _borrowedGames;
        private BaseRepository<UserType> _userTypes;

        public UnityOfWork(BorrowedGamesContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public IUserRepository Users => _users ??= new UserRepository(_dbcontext);
        public IGameRepository Games => _games ??= new GameRepository(_dbcontext);
        public IBorrowedGameRepository BorrowedGames => _borrowedGames ??= new BorrowedGameRepository(_dbcontext);
        public IBaseRepository<UserType> UserTypes => _userTypes ??= new BaseRepository<UserType>(_dbcontext);

        public async Task Commit()
        {
            await _dbcontext.SaveChangesAsync();
        }
    }
}
