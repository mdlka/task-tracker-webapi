using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Entities;
using TaskTracker.Services;

namespace TaskTracker.Controllers
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
            return Ok(ConvertToDto(await _boardService.GetBoard(boardId)));
        }

        [HttpGet]
        public IActionResult GetBoards()
        {
            return Ok(_boardService.GetBoards().Select(ConvertToDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] BoardSummaryDto boardSummary)
        {
            var newBoard = await _boardService.CreateBoard(boardSummary.Name);
            return CreatedAtAction(nameof(CreateBoard), newBoard.Id, ConvertToDto(newBoard));
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