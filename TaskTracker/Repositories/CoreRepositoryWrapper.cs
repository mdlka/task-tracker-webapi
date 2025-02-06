using TaskTracker.Entities;

namespace TaskTracker.Repositories
{
    public class CoreRepositoryWrapper : ICoreRepositoryWrapper
    {
        private readonly TodoContext _todoContext;

        public CoreRepositoryWrapper(TodoContext todoContext)
        {
            _todoContext = todoContext;
            Boards = new BoardRepository(todoContext);
            Items = new TodoItemRepository(todoContext);
        }

        public IBoardRepository Boards { get; }
        public ITodoItemRepository Items { get; }
        
        public async Task Save()
        {
            await _todoContext.SaveChangesAsync();
        }
    }
}