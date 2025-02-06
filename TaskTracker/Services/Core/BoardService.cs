using Microsoft.EntityFrameworkCore;
using TaskTracker.Entities;
using TaskTracker.Exceptions;

namespace TaskTracker.Services
{
    public class BoardService
    {
        private readonly TodoContext _context;
        private readonly CurrentUserService _currentUserService;

        public BoardService(TodoContext context, CurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Board> GetBoard(Guid boardId)
        {
            var board = await _context.Boards.AsNoTracking().FirstOrDefaultAsync(b => b.Id == boardId);

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
            
            return _context.Boards.Where(b => b.OwnerId == _currentUserService.GetUserId()).AsNoTracking();
        }

        public async Task<Board> CreateBoard(BoardSummaryDto boardDto)
        {
            if (_currentUserService.IsAnonymous)
                throw new UnauthorizedException();
            
            var board = new Board
            {
                Id = Guid.NewGuid(),
                OwnerId = _currentUserService.GetUserId(),
                Name = boardDto.Name
            };

            await _context.AddAsync(board);
            await _context.SaveChangesAsync();

            return board;
        }
    }
}