using TaskTracker.Core.Models;

namespace TaskTracker.Core.Repositories
{
    public interface IAuthRepositoryWrapper
    {
        IRepositoryBase<User> Users { get; }
        IRepositoryBase<UserCredentials> UserCredentials { get; }
        IRepositoryBase<RefreshToken> RefreshTokens { get; }

        Task Save();
    }
}