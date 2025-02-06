using TaskTracker.Core.Models;
using TaskTracker.Core.Repositories;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories
{
    public class AuthRepositoryWrapper : IAuthRepositoryWrapper
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AuthRepositoryWrapper(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Users = new RepositoryBase<User>(applicationDbContext);
            UserCredentials = new RepositoryBase<UserCredentials>(applicationDbContext);
            RefreshTokens = new RepositoryBase<RefreshToken>(applicationDbContext);
        }

        public IRepositoryBase<User> Users { get; }
        public IRepositoryBase<UserCredentials> UserCredentials { get; }
        public IRepositoryBase<RefreshToken> RefreshTokens { get; }

        public async Task Save()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}