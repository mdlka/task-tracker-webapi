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

        public Task<Board?> GetBoard(Guid id, Guid userId)
        {
            return _context.Boards.Where(b => b.OwnerId == userId).FirstOrDefaultAsync(board => board.Id == id);
        }

        public IEnumerable<Board> GetBoards(Guid userId)
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