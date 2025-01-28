using Microsoft.AspNetCore.Mvc;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Services;

namespace TaskTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoItemsService _todoItemsService;

        public TodoItemsController(TodoItemsService todoItemsService)
        {
            _todoItemsService = todoItemsService;
        }

        [HttpGet("{itemId:guid}")]
        public async Task<IActionResult> GetItem(Guid itemId)
        {
            var item = await _todoItemsService.GetItem(itemId);

            if (item == null)
                return NotFound();

            return Ok(ConvertToDto(item));
        }

        [HttpGet]
        public IActionResult GetItems([FromQuery] Guid boardId)
        {
            if (boardId == Guid.Empty)
                return NotFound();
            
            return Ok(_todoItemsService.GetItems(boardId).Select(ConvertToDto));
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromQuery] Guid boardId, [FromBody] TodoItemSummaryDto todoItemSummary)
        {
            if (boardId == Guid.Empty)
                return BadRequest();

            var newTodoItem = await _todoItemsService.CreateItem(todoItemSummary, boardId);
            return CreatedAtAction(nameof(PostItem), newTodoItem.Id, ConvertToDto(newTodoItem));
        }

        [HttpPut]
        public async Task<IActionResult> PutItem([FromBody] TodoItemDto todoItem)
        {
            return await _todoItemsService.UpdateItem(todoItem) ? Ok() : NotFound();
        }

        [HttpDelete("{itemId:guid}")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            return await _todoItemsService.DeleteItem(itemId) ? Ok() : NotFound();
        }

        private static TodoItemDto ConvertToDto(TodoItem item)
        {
            return new TodoItemDto
            {
                Id = item.Id,
                Name = item.Name,
                State = item.State
            };
        }
    }
}