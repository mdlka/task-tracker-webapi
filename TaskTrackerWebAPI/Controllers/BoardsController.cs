using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Services;

namespace TaskTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BoardsController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardsController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet("{boardId:guid}")]
        public async Task<IActionResult> GetBoard(Guid boardId)
        {
            var board = await _boardService.GetBoard(boardId);

            if (board == null)
                return NotFound();

            return Ok(ConvertToDto(board));
        }

        [HttpGet]
        public IActionResult GetBoards()
        {
            return Ok(_boardService.GetBoards().AsEnumerable().Select(ConvertToDto));
        }

        [HttpPost]
        public async Task<IActionResult> PostBoard([FromBody] BoardSummaryDto boardDto)
        {
            var board = await _boardService.CreateBoard(boardDto);
            
            if (board == null)
                return UnprocessableEntity();
            
            return CreatedAtAction(nameof(PostBoard), board.Id, ConvertToDto(board));
        }

        private static BoardDto ConvertToDto(Board board)
        {
            return new BoardDto
            {
                Id = board.Id,
                Name = board.Name
            };
        }
    }
}