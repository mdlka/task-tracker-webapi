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

            return Ok(item);
        }

        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(_todoItemsService.GetItems());
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] TodoItemSummaryDto todoItemSummary)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newTodoItem = await _todoItemsService.CreateItem(todoItemSummary);
            return CreatedAtAction(nameof(PostItem), newTodoItem.Id, newTodoItem);
        }

        [HttpDelete("{itemId:guid}")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            bool result = await _todoItemsService.DeleteItem(itemId);
            return result ? Ok() : NotFound();
        }
    }
}