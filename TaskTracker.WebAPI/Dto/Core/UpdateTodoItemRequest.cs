using System.ComponentModel.DataAnnotations;
using TaskTracker.Core.Models;

namespace TaskTracker.WebAPI.Dto
{
    public class UpdateTodoItemRequest
    {
        public Guid Id { get; set; }
        
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public TodoItemState State { get; set; }
    }
}