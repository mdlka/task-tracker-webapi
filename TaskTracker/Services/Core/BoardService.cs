﻿using TaskTracker.Entities;
using TaskTracker.Exceptions;
using TaskTracker.Repositories;

namespace TaskTracker.Services
{
    public class BoardService
    {
        private readonly ICoreRepositoryWrapper _repositoryWrapper;
        private readonly CurrentUserService _currentUserService;

        public BoardService(ICoreRepositoryWrapper repositoryWrapper, CurrentUserService currentUserService)
        {
            _repositoryWrapper = repositoryWrapper;
            _currentUserService = currentUserService;
        }

        public async Task<Board> GetBoard(Guid boardId)
        {
            var board = await _repositoryWrapper.Boards.FirstOrDefault(b => b.Id == boardId);

            if (board == null)
                throw new NotFoundException();
            
            if (_currentUserService.IsAnonymous)
                throw new UnauthorizedException();

            if (board.OwnerId != _currentUserService.GetUserId())
                throw new ForbiddenAccessException();
            
            return board;
        }

        public IEnumerable<Board> GetBoards()
        {
            if (_currentUserService.IsAnonymous)
                throw new UnauthorizedException();
            
            return _repositoryWrapper.Boards.FindAll(b => b.OwnerId == _currentUserService.GetUserId());
        }

        public async Task<Board> CreateBoard(string boardName)
        {
            if (_currentUserService.IsAnonymous)
                throw new UnauthorizedException();
            
            var board = new Board
            {
                Id = Guid.NewGuid(),
                OwnerId = _currentUserService.GetUserId(),
                Name = boardName
            };

            await _repositoryWrapper.Boards.Add(board);
            await _repositoryWrapper.Save();

            return board;
        }
    }
}