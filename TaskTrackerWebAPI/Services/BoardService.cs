using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;

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

        public async Task<Board?> GetBoard(Guid boardId)
        {
            return await GetBoards().FirstOrDefaultAsync(board => board.Id == boardId);
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