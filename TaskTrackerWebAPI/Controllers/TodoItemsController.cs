using Microsoft.AspNetCore.Mvc;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Services;

namespace TaskTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public IActionResult GetItems()
        {
            return Ok(_todoItemsService.GetItems().Select(ConvertToDto));
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] TodoItemSummaryDto todoItemSummary)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newTodoItem = await _todoItemsService.CreateItem(todoItemSummary);
            return CreatedAtAction(nameof(PostItem), newTodoItem.Id, ConvertToDto(newTodoItem));
        }

        [HttpPut]
        public async Task<IActionResult> PutItem([FromBody] TodoItemDto todoItem)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            bool result = await _todoItemsService.UpdateItem(todoItem);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{itemId:guid}")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            bool result = await _todoItemsService.DeleteItem(itemId);
            return result ? Ok() : NotFound();
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