using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;

namespace TaskTrackerWebAPI.Services
{
    public class TodoItemsService
    {
        private readonly TodoContext _context;

        public TodoItemsService(TodoContext context)
        {
            _context = context;
        }

        public Task<TodoItem?> GetItem(Guid id)
        {
            return _context.TodoItems.FirstOrDefaultAsync(item => item.Id == id);
        }

        public IEnumerable<TodoItem> GetItems()
        {
            return _context.TodoItems.AsNoTracking();
        }

        public async Task<TodoItem> CreateItem(TodoItemSummaryDto todoItemSummary)
        {
            var newTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Name = todoItemSummary.Name
            };

            await _context.TodoItems.AddAsync(newTodoItem);
            await _context.SaveChangesAsync();

            return newTodoItem;
        }

        public async Task<bool> DeleteItem(Guid id)
        {
            var item = await GetItem(id);

            if (item == null)
                return false;

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}