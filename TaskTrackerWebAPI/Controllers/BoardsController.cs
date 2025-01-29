using System.Security.Claims;
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
            if (!TryGetUserId(out var userId))
                return UnprocessableEntity();
            
            var board = await _boardService.GetBoard(boardId, userId);

            if (board == null)
                return NotFound();

            return Ok(ConvertToDto(board));
        }

        [HttpGet]
        public IActionResult GetBoards()
        {
            if (!TryGetUserId(out var userId))
                return UnprocessableEntity();
            
            return Ok(_boardService.GetBoards(userId).AsEnumerable().Select(ConvertToDto));
        }

        [HttpPost]
        public async Task<IActionResult> PostBoard([FromBody] BoardSummaryDto boardDto)
        {
            if (!TryGetUserId(out var userId))
                return UnprocessableEntity();
            
            var board = await _boardService.CreateBoard(boardDto, userId);
            return CreatedAtAction(nameof(PostBoard), board.Id, ConvertToDto(board));
        }

        private bool TryGetUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return false;

            userId = Guid.Parse(userIdClaim.Value);
            return true;
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