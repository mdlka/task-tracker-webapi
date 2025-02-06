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
            return Ok(ConvertToResponse(await _itemsService.GetItem(itemId)));
        }

        [HttpGet]
        public IActionResult GetItems([FromQuery] Guid boardId)
        {
            return Ok(_itemsService.GetItems(boardId).Select(ConvertToResponse));
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromQuery] Guid boardId, [FromBody] CreateTodoItemRequest itemRequest)
        {
            var newTodoItem = await _itemsService.CreateItem(itemRequest.Name, boardId);
            return CreatedAtAction(nameof(CreateItem), newTodoItem.Id, ConvertToResponse(newTodoItem));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateTodoItemRequest itemRequest)
        {
            await _itemsService.UpdateItem(itemRequest.Id, itemRequest.Name, itemRequest.State); 
            return NoContent();
        }

        [HttpDelete("{itemId:guid}")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            await _itemsService.DeleteItem(itemId);
            return NoContent();
        }

        private static TodoItemResponse ConvertToResponse(TodoItem item)
        {
            return new TodoItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                State = item.State
            };
        }
    }
}