using TaskTracker.Entities;

namespace TaskTracker.Repositories
{
    public class AuthRepositoryWrapper : IAuthRepositoryWrapper
    {
        private readonly TodoContext _todoContext;

        public AuthRepositoryWrapper(TodoContext todoContext)
        {
            _todoContext = todoContext;
            Users = new RepositoryBase<User>(todoContext);
            UserCredentials = new RepositoryBase<UserCredentials>(todoContext);
            RefreshTokens = new RepositoryBase<RefreshToken>(todoContext);
        }

        public IRepositoryBase<User> Users { get; }
        public IRepositoryBase<UserCredentials> UserCredentials { get; }
        public IRepositoryBase<RefreshToken> RefreshTokens { get; }

        public async Task Save()
        {
            await _todoContext.SaveChangesAsync();
        }
    }
}