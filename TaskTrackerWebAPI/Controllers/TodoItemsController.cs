using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Services;

namespace TaskTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            return Ok(ConvertToDto(await _todoItemsService.GetItem(itemId)));
        }

        [HttpGet]
        public IActionResult GetItems([FromQuery] Guid boardId)
        {
            return Ok(_todoItemsService.GetItems(boardId).Select(ConvertToDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromQuery] Guid boardId, [FromBody] TodoItemSummaryDto todoItemSummary)
        {
            var newTodoItem = await _todoItemsService.CreateItem(todoItemSummary, boardId);
            return CreatedAtAction(nameof(CreateItem), newTodoItem.Id, ConvertToDto(newTodoItem));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] TodoItemDto todoItem)
        {
            await _todoItemsService.UpdateItem(todoItem); 
            return NoContent();
        }

        [HttpDelete("{itemId:guid}")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            await _todoItemsService.DeleteItem(itemId);
            return NoContent();
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