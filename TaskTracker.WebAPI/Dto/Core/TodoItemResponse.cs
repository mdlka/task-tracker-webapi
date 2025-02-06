using TaskTracker.Core.Models;

namespace TaskTracker.WebAPI.Dto
{
    public class TodoItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TodoItemState State { get; set; }
    }
}