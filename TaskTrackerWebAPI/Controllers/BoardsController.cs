using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Services;

namespace TaskTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(_boardService.GetBoards().Select(ConvertToDto));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostBoard([FromBody] BoardSummaryDto boardDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var board = await _boardService.CreateBoard(boardDto);
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