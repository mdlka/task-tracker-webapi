using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWebAPI.Entities
{
    public class TodoItemDto
    {
        public Guid Id { get; set; }
        
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public TodoItemState State { get; set; }
    }
    
    public class TodoItemSummaryDto
    {
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }
}