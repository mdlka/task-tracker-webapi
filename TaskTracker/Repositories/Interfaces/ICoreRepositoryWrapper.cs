namespace TaskTracker.Repositories
{
    public interface ICoreRepositoryWrapper
    {
        IBoardRepository Boards { get; }
        ITodoItemRepository Items { get; }

        Task Save();
    }
}