using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Exceptions;

namespace TaskTrackerWebAPI.Services
{
    public class BoardService
    {
        private readonly TodoContext _context;
        private readonly UserService _userService;

        public BoardService(TodoContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Board> GetBoard(Guid boardId)
        {
            var board = await _context.Boards.AsNoTracking().FirstOrDefaultAsync(b => b.Id == boardId);

            if (board == null)
                throw new NotFoundException();

            if (!_userService.TryGetUserId(out var userId) || board.OwnerId != userId)
                throw new ForbiddenAccessException();
            
            return board;
        }

        public IQueryable<Board> GetBoards()
        {
            if (!_userService.TryGetUserId(out var userId))
                return Enumerable.Empty<Board>().AsQueryable();
            
            return _context.Boards.Where(b => b.OwnerId == userId).AsNoTracking();
        }

        public async Task<Board?> CreateBoard(BoardSummaryDto boardDto)
        {
            if (!_userService.TryGetUserId(out var userId))
                return null;
            
            var board = new Board
            {
                Id = Guid.NewGuid(),
                OwnerId = userId,
                Name = boardDto.Name
            };

            await _context.AddAsync(board);
            await _context.SaveChangesAsync();

            return board;
        }
    }
}