using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Core.Models;
using TaskTracker.Core.Services;
using TaskTracker.WebAPI.Dto;

namespace TaskTracker.WebAPI.Controllers
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
            return Ok(ConvertToResponse(await _boardService.GetBoard(boardId)));
        }

        [HttpGet]
        public IActionResult GetBoards()
        {
            return Ok(_boardService.GetBoards().Select(ConvertToResponse));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] BoardRequest boardRequest)
        {
            var newBoard = await _boardService.CreateBoard(boardRequest.Name);
            return CreatedAtAction(nameof(CreateBoard), newBoard.Id, ConvertToResponse(newBoard));
        }

        private static BoardResponse ConvertToResponse(Board board)
        {
            return new BoardResponse
            {
                Id = board.Id,
                Name = board.Name
            };
        }
    }
}