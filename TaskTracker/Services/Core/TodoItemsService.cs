using Microsoft.EntityFrameworkCore;
using TaskTracker.Entities;
using TaskTracker.Exceptions;

namespace TaskTracker.Services
{
    public class TodoItemsService
    {
        private readonly TodoContext _context;
        private readonly CurrentUserService _currentUserService;

        public TodoItemsService(TodoContext context, CurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TodoItem> GetItem(Guid itemId)
        {
            var item = await _context.TodoItems
                .Include(item => item.Board)
                .FirstOrDefaultAsync(item => item.Id == itemId);

            if (item == null)
                throw new NotFoundException();

            if (_currentUserService.IsAnonymous)
                throw new UnauthorizedException();
            
            if (item.Board.OwnerId != _currentUserService.GetUserId())
                throw new ForbiddenAccessException();
            
            return item;
        }

        public IEnumerable<TodoItem> GetItems(Guid boardId)
        {
            if (_currentUserService.IsAnonymous)
                throw new UnauthorizedException();
            
            return _context.TodoItems
                .Include(item => item.Board)
                .Where(item => item.BoardId == boardId && item.Board.OwnerId == _currentUserService.GetUserId())
                .AsNoTracking();
        }

        public async Task<TodoItem> CreateItem(string itemName, Guid boardId)
        {
            if (!await HasAccessToBoard(boardId))
                throw new ForbiddenAccessException();
            
            var newTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Name = itemName,
                BoardId = boardId
            };

            await _context.TodoItems.AddAsync(newTodoItem);
            await _context.SaveChangesAsync();

            return newTodoItem;
        }

        public async Task UpdateItem(Guid itemId, string newItemName, TodoItemState newItemState)
        {
            var item = await GetItem(itemId);

            item.Name = newItemName;
            item.State = newItemState;

            _context.TodoItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(Guid itemId)
        {
            var item = await GetItem(itemId);

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
        }
        
        private async Task<bool> HasAccessToBoard(Guid boardId)
        {
            if (_currentUserService.IsAnonymous)
                return false;

            var board = await _context.Boards.FirstOrDefaultAsync(board => board.Id == boardId);
            return board != null && board.OwnerId == _currentUserService.GetUserId();
        }
    }
}