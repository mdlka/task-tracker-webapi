using Microsoft.EntityFrameworkCore;
using TaskTracker.Entities;
using TaskTracker.Exceptions;

namespace TaskTracker.Services
{
    public class BoardService
    {
        private readonly TodoContext _context;
        private readonly UserContext _userContext;

        public BoardService(TodoContext context, UserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<Board> GetBoard(Guid boardId)
        {
            var board = await _context.Boards.AsNoTracking().FirstOrDefaultAsync(b => b.Id == boardId);

            if (board == null)
                throw new NotFoundException();
            
            if (_userContext.IsAnonymous)
                throw new UnauthorizedException();

            if (board.OwnerId != _userContext.GetUserId())
                throw new ForbiddenAccessException();
            
            return board;
        }

        public IEnumerable<Board> GetBoards()
        {
            if (_userContext.IsAnonymous)
                throw new UnauthorizedException();
            
            return _context.Boards.Where(b => b.OwnerId == _userContext.GetUserId()).AsNoTracking();
        }

        public async Task<Board> CreateBoard(BoardSummaryDto boardDto)
        {
            if (_userContext.IsAnonymous)
                throw new UnauthorizedException();
            
            var board = new Board
            {
                Id = Guid.NewGuid(),
                OwnerId = _userContext.GetUserId(),
                Name = boardDto.Name
            };

            await _context.AddAsync(board);
            await _context.SaveChangesAsync();

            return board;
        }
    }
}