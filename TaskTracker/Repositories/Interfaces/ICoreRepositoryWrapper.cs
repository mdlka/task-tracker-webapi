using TaskTracker.Entities;

namespace TaskTracker.Repositories
{
    public interface ICoreRepositoryWrapper
    {
        IRepositoryBase<Board> Boards { get; }
        IRepositoryBase<TodoItem> Items { get; }

        Task Save();
    }
}