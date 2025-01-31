using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;

namespace TaskTrackerWebAPI.Services
{
    public class TodoItemsService
    {
        private readonly TodoContext _context;
        private readonly UserService _userService;

        public TodoItemsService(TodoContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<TodoItem?> GetItem(Guid itemId)
        {
            if (!_userService.TryGetUserId(out var userId))
                return null;
            
            return await _context.TodoItems
                .Include(item => item.Board)
                .FirstOrDefaultAsync(item => item.Id == itemId && item.Board.OwnerId == userId);
        }

        public IEnumerable<TodoItem> GetItems(Guid boardId)
        {
            if (!_userService.TryGetUserId(out var userId))
                return Array.Empty<TodoItem>();
            
            return _context.TodoItems
                .Include(item => item.Board)
                .Where(item => item.BoardId == boardId && item.Board.OwnerId == userId)
                .AsNoTracking();
        }

        public async Task<TodoItem?> CreateItem(TodoItemSummaryDto todoItemSummary, Guid boardId)
        {
            if (!await _userService.HasAccessToBoard(boardId))
                return null;
            
            var newTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Name = todoItemSummary.Name,
                BoardId = boardId
            };

            await _context.TodoItems.AddAsync(newTodoItem);
            await _context.SaveChangesAsync();

            return newTodoItem;
        }

        public async Task<bool> UpdateItem(TodoItemDto itemDto)
        {
            if (!await _userService.HasAccessToTodoItem(itemDto.Id))
                return false;
            
            var item = await GetItem(itemDto.Id);

            if (item == null)
                return false;

            item.Name = itemDto.Name;
            item.State = itemDto.State;

            _context.TodoItems.Update(item);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteItem(Guid itemId)
        {
            if (!await _userService.HasAccessToTodoItem(itemId))
                return false;
            
            var item = await GetItem(itemId);

            if (item == null)
                return false;

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}