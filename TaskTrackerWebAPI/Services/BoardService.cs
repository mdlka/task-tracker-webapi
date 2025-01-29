using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;

namespace TaskTrackerWebAPI.Services
{
    public class BoardService
    {
        private readonly TodoContext _context;

        public BoardService(TodoContext context)
        {
            _context = context;
        }

        public Task<Board?> GetBoard(Guid boardId, Guid userId)
        {
            return GetBoards(userId).FirstOrDefaultAsync(board => board.Id == boardId);
        }

        public IQueryable<Board> GetBoards(Guid userId)
        {
            return _context.Boards.Where(b => b.OwnerId == userId).AsNoTracking();
        }

        public async Task<Board> CreateBoard(BoardSummaryDto boardDto, Guid userId)
        {
            var board = new Board()
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