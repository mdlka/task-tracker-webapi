using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Exceptions;

namespace TaskTrackerWebAPI.Services
{
    public class TodoItemsService
    {
        private readonly TodoContext _context;
        private readonly UserContext _userContext;

        public TodoItemsService(TodoContext context, UserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<TodoItem> GetItem(Guid itemId)
        {
            var item = await _context.TodoItems
                .Include(item => item.Board)
                .FirstOrDefaultAsync(item => item.Id == itemId);

            if (item == null)
                throw new NotFoundException();

            if (_userContext.IsAnonymous)
                throw new UnauthorizedException();
            
            if (item.Board.OwnerId != _userContext.GetUserId())
                throw new ForbiddenAccessException();
            
            return item;
        }

        public IEnumerable<TodoItem> GetItems(Guid boardId)
        {
            if (_userContext.IsAnonymous)
                throw new UnauthorizedException();
            
            return _context.TodoItems
                .Include(item => item.Board)
                .Where(item => item.BoardId == boardId && item.Board.OwnerId == _userContext.GetUserId())
                .AsNoTracking();
        }

        public async Task<TodoItem> CreateItem(TodoItemSummaryDto todoItemSummary, Guid boardId)
        {
            if (!await HasAccessToBoard(boardId))
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
            var item = await GetItem(itemDto.Id);

            item.Name = itemDto.Name;
            item.State = itemDto.State;

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
            if (_userContext.IsAnonymous)
                return false;

            var board = await _context.Boards.FirstOrDefaultAsync(board => board.Id == boardId);
            return board != null && board.OwnerId == _userContext.GetUserId();
        }
    }
}