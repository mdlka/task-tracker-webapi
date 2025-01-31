using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Extensions;

namespace TaskTrackerWebAPI.Services
{
    public class UserService
    {
        private readonly ClaimsPrincipal _user;
        private readonly TodoContext _bdContext;

        public UserService(ClaimsPrincipal user, TodoContext bdContext)
        {
            _user = user;
            _bdContext = bdContext;
        }

        public bool TryGetUserId(out Guid userId)
        {
            return _user.TryGetUserId(out userId);
        }

        public async Task<bool> HasAccessToTodoItem(Guid itemId)
        {
            if (!_user.TryGetUserId(out var userId))
                return false;

            var todoItem = await _bdContext.TodoItems
                .Include(todoItem => todoItem.Board)
                .FirstOrDefaultAsync(item => item.Id == itemId);
            
            return todoItem != null && todoItem.Board.OwnerId == userId;
        }

        public async Task<bool> HasAccessToBoard(Guid boardId)
        {
            if (!_user.TryGetUserId(out var userId))
                return false;

            var board = await _bdContext.Boards.FirstOrDefaultAsync(board => board.Id == boardId);
            return board != null && board.OwnerId == userId;
        }
    }
}