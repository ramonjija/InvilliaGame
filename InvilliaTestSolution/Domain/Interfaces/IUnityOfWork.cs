using Domain.Model.Aggregate;
using Domain.Model.Entity;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnityOfWork
    {
        IUserRepository Users { get; }
        IGameRepository Games { get; }
        IBorrowedGameRepository BorrowedGames { get; }
        IBaseRepository<UserType> UserTypes { get; }

        Task Commit();
    }
}
