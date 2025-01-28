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
            Guid? userId = GetUserId();

            if (userId == null)
                return UnprocessableEntity();
            
            var board = await _boardService.GetBoard(boardId, userId.Value);

            if (board == null)
                return NotFound();

            return Ok(ConvertToDto(board));
        }

        [HttpGet]
        public IActionResult GetBoards()
        {
            Guid? userId = GetUserId();

            if (userId == null)
                return UnprocessableEntity();
            
            return Ok(_boardService.GetBoards(userId.Value).Select(ConvertToDto));
        }

        [HttpPost]
        public async Task<IActionResult> PostBoard([FromBody] BoardSummaryDto boardDto)
        {
            Guid? userId = GetUserId();

            if (userId == null)
                return UnprocessableEntity();
            
            var board = await _boardService.CreateBoard(boardDto, userId.Value);
            return CreatedAtAction(nameof(PostBoard), board.Id, ConvertToDto(board));
        }

        private Guid? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            return Guid.Parse(userIdClaim.Value);
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