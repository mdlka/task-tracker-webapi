using TaskTracker.Entities;

namespace TaskTracker.Repositories
{
    public class TodoItemRepository : RepositoryBase<TodoItem>, ITodoItemRepository
    {
        public TodoItemRepository(TodoContext todoContext) : base(todoContext) { }
    }
}