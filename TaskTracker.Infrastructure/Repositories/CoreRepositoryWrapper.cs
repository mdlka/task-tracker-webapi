using TaskTracker.Core.Models;
using TaskTracker.Core.Repositories;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories
{
    public class CoreRepositoryWrapper : ICoreRepositoryWrapper
    {
        private readonly TodoContext _todoContext;

        public CoreRepositoryWrapper(TodoContext todoContext)
        {
            _todoContext = todoContext;
            Boards = new RepositoryBase<Board>(todoContext);
            Items = new RepositoryBase<TodoItem>(todoContext);
        }

        public IRepositoryBase<Board> Boards { get; }
        public IRepositoryBase<TodoItem> Items { get; }

        public async Task Save()
        {
            await _todoContext.SaveChangesAsync();
        }
    }
}