using TaskTracker.Core.Models;
using TaskTracker.Core.Repositories;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories
{
    public class CoreRepositoryWrapper : ICoreRepositoryWrapper
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CoreRepositoryWrapper(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Boards = new RepositoryBase<Board>(applicationDbContext);
            Items = new RepositoryBase<TodoItem>(applicationDbContext);
        }

        public IRepositoryBase<Board> Boards { get; }
        public IRepositoryBase<TodoItem> Items { get; }

        public async Task Save()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}