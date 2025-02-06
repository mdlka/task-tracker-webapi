using TaskTracker.Core.Models;

namespace TaskTracker.Core.Repositories
{
    public interface ICoreRepositoryWrapper
    {
        IRepositoryBase<Board> Boards { get; }
        IRepositoryBase<TodoItem> Items { get; }

        Task Save();
    }
}