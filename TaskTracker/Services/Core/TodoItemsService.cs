using Microsoft.EntityFrameworkCore;
using TaskTracker.Entities;
using TaskTracker.Exceptions;
using TaskTracker.Repositories;

namespace TaskTracker.Services
{
    public class TodoItemsService
    {
        private readonly ICoreRepositoryWrapper _repositoryWrapper;
        private readonly CurrentUserService _currentUserService;

        public TodoItemsService(ICoreRepositoryWrapper repositoryWrapper, CurrentUserService currentUserService)
        {
            _repositoryWrapper = repositoryWrapper;
            _currentUserService = currentUserService;
        }

        public async Task<TodoItem> GetItem(Guid itemId)
        {
            var item = await _repositoryWrapper.Items.FirstOrDefault(item => item.Id == itemId, 
                                    with: item => item.Board);

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

            return _repositoryWrapper.Items.FindAll(
                item => item.BoardId == boardId && item.Board.OwnerId == _currentUserService.GetUserId(),
                with: item => item.Board);
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

            await _repositoryWrapper.Items.Add(newTodoItem);
            await _repositoryWrapper.Save();

            return newTodoItem;
        }

        public async Task UpdateItem(Guid itemId, string newItemName, TodoItemState newItemState)
        {
            var item = await GetItem(itemId);

            item.Name = newItemName;
            item.State = newItemState;

            _repositoryWrapper.Items.Update(item);
            await _repositoryWrapper.Save();
        }

        public async Task DeleteItem(Guid itemId)
        {
            var item = await GetItem(itemId);
            
            _repositoryWrapper.Items.Delete(item);
            await _repositoryWrapper.Save();
        }
        
        private async Task<bool> HasAccessToBoard(Guid boardId)
        {
            if (_currentUserService.IsAnonymous)
                return false;

            var board = await _repositoryWrapper.Boards.FirstOrDefault(board => board.Id == boardId);
            return board != null && board.OwnerId == _currentUserService.GetUserId();
        }
    }
}