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

        public Task<Board?> GetBoard(Guid id)
        {
            return _context.Boards.FirstOrDefaultAsync(board => board.Id == id);
        }

        public IEnumerable<Board> GetBoards()
        {
            return _context.Boards.AsNoTracking();
        }

        public async Task<Board> CreateBoard(BoardSummaryDto boardDto)
        {
            var board = new Board()
            {
                Id = Guid.NewGuid(),
                Name = boardDto.Name
            };

            await _context.AddAsync(board);
            await _context.SaveChangesAsync();

            return board;
        }
    }
}