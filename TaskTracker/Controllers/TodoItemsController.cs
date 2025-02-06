using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Entities;
using TaskTracker.Services;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoItemsService _itemsService;

        public TodoItemsController(TodoItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        [HttpGet("{itemId:guid}")]
        public async Task<IActionResult> GetItem(Guid itemId)
        {
            return Ok(ConvertToDto(await _itemsService.GetItem(itemId)));
        }

        [HttpGet]
        public IActionResult GetItems([FromQuery] Guid boardId)
        {
            return Ok(_itemsService.GetItems(boardId).Select(ConvertToDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromQuery] Guid boardId, [FromBody] TodoItemSummaryDto itemSummary)
        {
            var newTodoItem = await _itemsService.CreateItem(itemSummary.Name, boardId);
            return CreatedAtAction(nameof(CreateItem), newTodoItem.Id, ConvertToDto(newTodoItem));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] TodoItemDto item)
        {
            await _itemsService.UpdateItem(item.Id, item.Name, item.State); 
            return NoContent();
        }

        [HttpDelete("{itemId:guid}")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            await _itemsService.DeleteItem(itemId);
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