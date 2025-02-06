using TaskTracker.Entities;

namespace TaskTracker.Repositories
{
    public interface IAuthRepositoryWrapper
    {
        IRepositoryBase<User> Users { get; }
        IRepositoryBase<UserCredentials> UserCredentials { get; }
        IRepositoryBase<RefreshToken> RefreshTokens { get; }

        Task Save();
    }
}