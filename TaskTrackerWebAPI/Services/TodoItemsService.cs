using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Exceptions;

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

        public async Task<TodoItem> GetItem(Guid itemId)
        {
            var item = await _context.TodoItems
                .Include(item => item.Board)
                .FirstOrDefaultAsync(item => item.Id == itemId);

            if (item == null)
                throw new NotFoundException();
            
            if (!_userService.TryGetUserId(out var userId) || item.Board.OwnerId != userId)
                throw new ForbiddenAccessException();
            
            return item;
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

        public async Task<TodoItem> CreateItem(TodoItemSummaryDto todoItemSummary, Guid boardId)
        {
            if (!await _userService.HasAccessToBoard(boardId))
                throw new ForbiddenAccessException();
            
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

        public async Task UpdateItem(TodoItemDto itemDto)
        {
            if (!await _userService.HasAccessToTodoItem(itemDto.Id))
                throw new ForbiddenAccessException();
            
            var item = await GetItem(itemDto.Id);

            item.Name = itemDto.Name;
            item.State = itemDto.State;

            _context.TodoItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(Guid itemId)
        {
            if (!await _userService.HasAccessToTodoItem(itemId))
                throw new ForbiddenAccessException();
            
            var item = await GetItem(itemId);

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}